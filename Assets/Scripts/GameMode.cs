using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameMode : NetworkBehaviour
{
    [SerializeField] private float _restartDuration = 5;
    [SerializeField] private int _scoreToWin = 3;
    [SerializeField] private NetRoomManager _networkManager;

    private readonly List<Player.Player> _currentPlayers = new List<Player.Player>();

    public readonly SyncList<uint> CurrentPlayersIds = new SyncList<uint>();

    private bool _gameEnded;

    private void Awake()
    {
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
        var player = networkIdentity.GetComponent<Player.Player>();

        player.OnScoreChanged += OnPlayerScoreChanged;
        player.OnNameChanged += OnPlayerNameChanged;

        player.Name = "Player " + _currentPlayers.Count;

        CurrentPlayersIds.Add(networkIdentity.netId);
        _currentPlayers.Add(player);
    }

    [Server]
    private void OnServerPlayerDisconnected(NetworkIdentity networkIdentity)
    {
        var player = networkIdentity.GetComponent<Player.Player>();

        _currentPlayers.Remove(player);
        CurrentPlayersIds.Remove(networkIdentity.netId);

        player.OnScoreChanged -= OnPlayerScoreChanged;
        player.OnNameChanged -= OnPlayerNameChanged;
    }

    [Server]
    private void OnPlayerScoreChanged(Player.Player player)
    {
        if (_gameEnded) return;

        RpcPlayerScoreChanged();

        if (player.Score >= _scoreToWin)
        {
            FinishTheGame(player.Name);
        }
    }

    [ClientRpc]
    private void OnPlayerNameChanged(Player.Player player) => ClientPlayerNameChanged?.Invoke();

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

        foreach (Player.Player somePlayer in _currentPlayers)
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
        foreach (Player.Player player in _currentPlayers)
        {
            Transform randomSpot = _networkManager.GetStartPosition();
            player.SetPosition(randomSpot.position);
        }
    }

    [ClientRpc]
    private void RPCGameEnded(string winnerName) =>
        GameEnded?.Invoke(winnerName);

    [ClientRpc]
    private void RPCGameRestarted() =>
        GameRestart?.Invoke();
}