using System;
using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;

namespace UI
{
    public class ScoreLeaderboard : MonoBehaviour
    {
        [SerializeField] private ScoreLine _linePrefab;
        [SerializeField] private GameMode _gameMode;
        private Coroutine _refreshRoutine;

        private void Start()
        {
            _gameMode.ClientPlayerScoreChanged += Refresh;
            _gameMode.GameRestart += Refresh;
            _gameMode.ClientPlayerNameChanged += Refresh;
            _gameMode.CurrentPlayersIds.Callback += OnCallbackRefresh;
        }

        private void OnDestroy()
        {
            _gameMode.ClientPlayerScoreChanged -= Refresh;
            _gameMode.GameRestart -= Refresh;
            _gameMode.ClientPlayerNameChanged -= Refresh;
            _gameMode.CurrentPlayersIds.Callback -= OnCallbackRefresh;
        }

        private void OnCallbackRefresh(SyncList<uint>.Operation operation, int itemindex, uint olditem, uint newitem) =>
            // Debug.Log($"Refresh on Player. Op {operation}, index {itemindex}, old {olditem}, new {newitem}");
            Refresh();

        private void Refresh() =>
            _refreshRoutine ??= StartCoroutine(RefreshRoutine());

        private IEnumerator RefreshRoutine()
        {
            yield return null;
            var tryCount = 0;
            while (!_gameMode.CurrentPlayersIds.All(netId => NetworkClient.spawned.ContainsKey(netId)))
            {
                yield return null;
                tryCount++;
                if (tryCount > 5)
                {
                    Debug.LogWarning("Failed to refresh UI");
                    yield break;
                }
            }

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Debug.Log($"Refresh! List length is {_gameMode.CurrentPlayersIds.Count}");
            foreach (uint playerId in _gameMode.CurrentPlayersIds)
            {
                var player = NetworkClient.spawned[playerId].GetComponent<Player.Player>();

                ScoreLine line = Instantiate(_linePrefab, transform);
                line.Init(player.Name, player.Score.ToString());
            }

            _refreshRoutine = null;
        }
    }
}