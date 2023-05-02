using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Room;
using UnityEngine;
using UnityEngine.Assertions;

public class GameMode : NetworkBehaviour
{
    [SerializeField] private float _restartDuration = 5;
    [SerializeField] private int _scoreToWin = 3;
    [SerializeField] private NetRoomManager _networkManager;
    private readonly List<Player> _currentPlayersListPrivate = new List<Player>();

    public readonly SyncList<uint> CurrentPlayersBaseList = new SyncList<uint>();

    private bool _gameEnded;
    public static GameMode Instance { get; private set; }

    private void Awake()
    {
        Assert.IsNull(Instance);
        Instance = this;

        Application.targetFrameRate = 60;
        _networkManager = (NetRoomManager)NetworkManager.singleton;
    }

    public event Action<string> GameEnded;
    public event Action GameRestart;
    public event Action ClientPlayerScoreChanged;
    public event Action ClientPlayerNameChanged;

    public override void OnStartServer()
    {
        base.OnStartServer();
        _networkManager.ServerReady += OnServerPlayerReady;
        _networkManager.ServerPlayerDisconnected += OnServerPlayerDisconnected;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        _networkManager.ServerReady -= OnServerPlayerReady;
        _networkManager.ServerPlayerDisconnected -= OnServerPlayerDisconnected;
    }

    [Server]
    private void OnServerPlayerReady(NetworkIdentity networkIdentity)
    {
        var player = networkIdentity.GetComponent<Player>();

        player.OnScoreChanged += OnPlayerScoreChanged;
        player.OnNameChanged += OnPlayerNameChanged;

        player.Name = "Player " + _currentPlayersListPrivate.Count;

        CurrentPlayersBaseList.Add(networkIdentity.netId);
        _currentPlayersListPrivate.Add(player);

        Debug.Log("Adding player");
    }

    [Server]
    private void OnServerPlayerDisconnected(NetworkIdentity networkIdentity)
    {
        var player = networkIdentity.GetComponent<Player>();

        _currentPlayersListPrivate.Remove(player);
        CurrentPlayersBaseList.Remove(networkIdentity.netId);

        player.OnScoreChanged -= OnPlayerScoreChanged;
        player.OnNameChanged -= OnPlayerNameChanged;
    }

    [Server]
    private void OnPlayerScoreChanged(Player player)
    {
        if (_gameEnded) return;

        RpcPlayerScoreChanged();

        if (player.Score >= _scoreToWin)
        {
            FinishTheGame(player.Name);
        }
    }

    [ClientRpc]
    private void OnPlayerNameChanged(Player player) => ClientPlayerNameChanged?.Invoke();

    [ClientRpc]
    private void RpcPlayerScoreChanged() =>
        ClientPlayerScoreChanged?.Invoke();

    [Server]
    private void FinishTheGame(string winnerName)
    {
        _gameEnded = true;
        RPCGameEnded(winnerName);

        StartCoroutine(ProcessEndGame());
    }

    [Server]
    private IEnumerator ProcessEndGame()
    {
        yield return new WaitForSecondsRealtime(_restartDuration);

        foreach (Player somePlayer in _currentPlayersListPrivate)
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
        foreach (Player player in _currentPlayersListPrivate)
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
}