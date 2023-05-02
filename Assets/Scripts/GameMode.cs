using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.Assertions;

public class GameMode : NetworkBehaviour
{
    public static GameMode Instance { get; private set; }
    [SerializeField] private float _restartDuration = 5;
    [SerializeField] private int _scoreToWin = 3;
    [SerializeField] private NetManager _networkManager;

    public readonly SyncList<PlayerInfo> CurrentPlayersList = new SyncList<PlayerInfo>();
    private readonly List<PlayerBase> _currentPlayersListPrivate = new List<PlayerBase>();

    private bool _gameEnded;

    private void Awake()
    {
        Assert.IsNull(Instance);
        Instance = this;
    }

    private void Start() =>
        Application.targetFrameRate = 60;

    public event Action<string> GameEnded;
    public event Action GameRestart;
    public event Action ClientPlayerScoreChanged;

    public override void OnStartServer()
    {
        base.OnStartServer();
        _networkManager.ServerPlayerConnected += OnServerPlayerConnected;
        _networkManager.ServerPlayerDisconnected += OnServerPlayerDisconnected;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        _networkManager.ServerPlayerConnected -= OnServerPlayerConnected;
        _networkManager.ServerPlayerDisconnected -= OnServerPlayerDisconnected;
    }

    [Server]
    private void OnServerPlayerConnected(NetworkIdentity networkIdentity)
    {
        var player = networkIdentity.GetComponent<PlayerBase>();
        player.Name = "Player " + CurrentPlayersList.Count;
        CurrentPlayersList.Add(new PlayerInfo(networkIdentity.netId, "Player " + CurrentPlayersList.Count));
        _currentPlayersListPrivate.Add(player);
        Debug.Log("Adding player");
    }

    [Server]
    private void OnServerPlayerDisconnected(NetworkIdentity networkIdentity)
    {
        var player = networkIdentity.GetComponent<PlayerBase>();
        int bRemove =
            CurrentPlayersList.RemoveAll(info => info.Id == networkIdentity.netId);
        _currentPlayersListPrivate.Remove(player);
        if (bRemove == 1)
        {
            Debug.Log("Removing player");
        }
        else
        {
            Debug.LogWarning(
                $"Failed to remove player. Removed count is {bRemove}. Info list count is {CurrentPlayersList.Count}. Private list count is {_currentPlayersListPrivate.Count}");
        }
    }

    [Server]
    private void HandleScoreChange(PlayerBase playerBase)
    {
        if (_gameEnded) return;

        if (CurrentPlayersList.Count == 0) return;

        // RpcPlayerScoreChanged();

        PlayerInfo topPlayer = CurrentPlayersList.Aggregate((cur, next) => next.Score > cur.Score ? next : cur);

        if (topPlayer.Score >= _scoreToWin)
        {
            FinishTheGame(topPlayer.Name);
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

        // foreach (PlayerBase somePlayer in CurrentPlayersList)
        // {
        //     somePlayer.ResetScore();
        // }

        foreach (PlayerInfo info in CurrentPlayersList)
        {
            info.Score = 0;
        }

        RespawnCurrentPlayers();
        RPCGameRestarted();
        _gameEnded = false;
    }

    private void RespawnCurrentPlayers()
    {
        _networkManager.ResetUnusedStartPositions();
        foreach (PlayerBase player in _currentPlayersListPrivate)
        {
            Transform randomSpot = _networkManager.GetStartPosition();
            player.SetPosition(randomSpot.position);
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

    [Server]
    public void OnPlayerMadeHit(uint id, PlayerBase playerBase)
    {
        PlayerInfo found = CurrentPlayersList.Find(info => info.Id == id);
        if (found == null)
        {
            Debug.LogWarning($"Did not find PlayerInfo when incrementing score. Info count is {CurrentPlayersList.Count}");
            return;
        }
        found.Score++;
        Debug.Log($"Incrementing player score on server. Should be synced. New score is {found.Score}"); // bug не синкается на клиенте
        HandleScoreChange(playerBase);
    }
}