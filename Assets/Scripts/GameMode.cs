using System;
using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;

public class GameMode : NetworkBehaviour
{
    [SerializeField] private float _restartDuration = 5;
    [SerializeField] private int _scoreToWin = 1;
    [SerializeField] private NetManager _networkManager;

    // public IList<PlayerBase> CurrentPlayersList => _networkManager.CurrentPlayers;
    public readonly SyncList<PlayerInfo> CurrentPlayersInfoList = new SyncList<PlayerInfo>();

    // public IList<PlayerBase> CurrentPlayersList { get; private set; }
    public readonly SyncList<PlayerBase> CurrentPlayersList = new SyncList<PlayerBase>();

    private bool _gameEnded;

    private void Start() =>
        Application.targetFrameRate = 60;

    public event Action<string> GameEnded;
    public event Action GameRestart;
    public event Action ClientPlayerScoreChanged;

    public override void OnStartServer()
    {
        base.OnStartServer();

        // CurrentPlayersList = _networkManager.CurrentPlayers;

        _networkManager.ServerPlayerConnected += OnServerPlayerConnected;
        _networkManager.ServerPlayerDisconnected += OnServerPlayerDisconnected;
    }

    [Server]
    private void OnServerPlayerConnected(PlayerBase player)
    {
        player.Name = "Player " + CurrentPlayersList.Count;
        CurrentPlayersList.Add(player);
        Debug.Log("Adding player");
        player.OnScoreChanged += HandleScoreChange;
    }

    [Server]
    private void OnServerPlayerDisconnected(PlayerBase player)
    {
        bool bRemove = CurrentPlayersList.Remove(player);
        if (bRemove)
        {
            Debug.Log("Removing player");
        }
        else
        {
            Debug.LogWarning("Failed to remove player");
        }

        player.OnScoreChanged -= HandleScoreChange;
    }

    [Server]
    private void HandleScoreChange(PlayerBase playerBase)
    {
        if (_gameEnded) return;

        if (CurrentPlayersList.Count == 0) return;

        RpcPlayerScoreChanged();

        // PlayerBase topPlayer = _currentPlayersList.OrderByDescending(x => x.Score).First();
        PlayerBase topPlayer = CurrentPlayersList.Aggregate((cur, next) => next.Score > cur.Score ? next : cur);

        if (topPlayer.Score >= _scoreToWin)
        {
            FinishTheGame(topPlayer.name);
        }
    }

    [ClientRpc]
    private void RpcPlayerScoreChanged() =>
        ClientPlayerScoreChanged?.Invoke();

    [Server]
    private void FinishTheGame(string winnerName)
    {
        Debug.Log("Game ended");
        _gameEnded = true;
        RPCGameEnded(winnerName);

        StartCoroutine(ProcessEndGame());
    }

    [Server]
    private IEnumerator ProcessEndGame()
    {
        yield return new WaitForSecondsRealtime(_restartDuration);

        foreach (PlayerBase somePlayer in CurrentPlayersList)
        {
            somePlayer.ResetScore();
        }

        RespawnCurrentPlayers();
        RPCGameRestarted();
        _gameEnded = false;
    }

    private void RespawnCurrentPlayers()
    {
        _networkManager.ResetUnusedStartPositions();
        foreach (PlayerBase somePlayer in CurrentPlayersList)
        {
            Transform randomSpot = _networkManager.GetStartPosition();
            somePlayer.SetPosition(randomSpot.position);
        }
    }

    [ClientRpc]
    private void RPCGameEnded(string winnerName)
    {
        Debug.Log("Rpc GameEnded!");
        GameEnded?.Invoke(winnerName);
    }

    [ClientRpc]
    private void RPCGameRestarted()
    {
        Debug.Log("Rpc GameRestarted!");
        GameRestart?.Invoke();
    }
}

public struct PlayerInfo
{
    public string Name;
    public int Score;
}