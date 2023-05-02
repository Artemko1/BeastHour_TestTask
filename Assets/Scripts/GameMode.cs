using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Assertions;

public class GameMode : NetworkBehaviour
{
    public static GameMode Instance { get; private set; }
    [SerializeField] private float _restartDuration = 5;
    [SerializeField] private int _scoreToWin = 3;
    [SerializeField] private NewNetworkRoomManager _networkManager;

    public readonly SyncList<uint> CurrentPlayersBaseList = new SyncList<uint>();
    // public readonly SyncList<PlayerInfo> CurrentPlayersList = new SyncList<PlayerInfo>();
    private readonly List<PlayerBase> _currentPlayersListPrivate = new List<PlayerBase>();

    private bool _gameEnded;

    private void Awake()
    {
        Assert.IsNull(Instance);
        Instance = this;
        
        Application.targetFrameRate = 60;
        _networkManager = (NewNetworkRoomManager)NetworkManager.singleton;
    }

    public event Action<string> GameEnded;
    public event Action GameRestart;
    public event Action ClientPlayerScoreChanged;

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
        var player = networkIdentity.GetComponent<PlayerBase>();
        CurrentPlayersBaseList.Add(networkIdentity.netId);
        _currentPlayersListPrivate.Add(player);
        player.OnScoreChanged += OnPlayerScoreChanged;
        Debug.Log("Adding player");
    }

    [Server]
    private void OnServerPlayerDisconnected(NetworkIdentity networkIdentity)
    {
        var player = networkIdentity.GetComponent<PlayerBase>();
        _currentPlayersListPrivate.Remove(player);
        CurrentPlayersBaseList.Remove(networkIdentity.netId);
        player.OnScoreChanged -= OnPlayerScoreChanged;
    }

    [Server]
    private void OnPlayerScoreChanged(PlayerBase playerBase)
    {
        if (_gameEnded) return;

        // if (_currentPlayersListPrivate.Count == 0) return;

        RpcPlayerScoreChanged();

        // PlayerBase topPlayer = _currentPlayersListPrivate.Aggregate((cur, next) => next.Score > cur.Score ? next : cur);

        if (playerBase.Score >= _scoreToWin)
        {
            FinishTheGame(playerBase.name);
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

        foreach (PlayerBase somePlayer in _currentPlayersListPrivate)
        {
            somePlayer.ResetScore();
        }

        // foreach (PlayerInfo info in CurrentPlayersList)
        // {
        //     info.Score = 0;
        // }

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
        // PlayerInfo found = CurrentPlayersList.Find(info => info.Id == id);
        // if (found == null)
        // {
        //     Debug.LogWarning($"Did not find PlayerInfo when incrementing score. Info count is {CurrentPlayersList.Count}");
        //     return;
        // }
        // found.Score++;
        // Debug.Log($"Incrementing player score on server. Should be synced. New score is {found.Score}"); // bug не синкается на клиенте
        OnPlayerScoreChanged(playerBase);
    }
}