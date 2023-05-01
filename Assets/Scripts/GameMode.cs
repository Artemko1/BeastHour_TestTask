using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class GameMode : NetworkBehaviour
{
    [SerializeField] private float _restartDuration = 5;
    [SerializeField] private int _scoreToWin = 3;

    private IList<PlayerBase> _currentPlayersList;

    private bool _gameEnded;
    private NetManager _networkManager;

    private void Start() =>
        Application.targetFrameRate = 60;

    public static event Action<string> OnGameEnded;
    public static event Action OnGameStarted;

    public override void OnStartServer()
    {
        base.OnStartServer();

        _networkManager = (NetManager)NetworkManager.singleton;
        _currentPlayersList = _networkManager.CurrentPlayers;

        _networkManager.PlayerConnected += player => player.OnScoreChanged2 += HandleScoreChange;
        _networkManager.PlayerDisonnected += player => player.OnScoreChanged2 -= HandleScoreChange;
    }

    [Server]
    private void HandleScoreChange()
    {
        if (_gameEnded) return;

        if (_currentPlayersList.Count == 0) return;

        // PlayerBase topPlayer = _currentPlayersList.OrderByDescending(x => x.Score).First();
        PlayerBase topPlayer = _currentPlayersList.Aggregate((cur, next) => next.Score > cur.Score ? next : cur);

        if (topPlayer.Score >= _scoreToWin)
        {
            FinishTheGame(topPlayer.name);
        }
    }

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

        foreach (PlayerBase somePlayer in _currentPlayersList)
        {
            somePlayer.ResetScore();
        }

        _networkManager.RespawnCurrentPlayers();
        RPCGameRestarted();
        _gameEnded = false;
    }

    [ClientRpc]
    private void RPCGameEnded(string winnerName)
    {
        Debug.Log("Rpc GameEnded!");
        OnGameEnded?.Invoke(winnerName);
    }

    [ClientRpc]
    private void RPCGameRestarted()
    {
        Debug.Log("Rpc GameRestarted!");
        OnGameStarted?.Invoke();
    }
}