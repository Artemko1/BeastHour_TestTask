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

        private void Start()
        {
            _gameMode.ClientPlayerScoreChanged += Refresh;
            _gameMode.CurrentPlayersBaseList.Callback += OnCallbackRefresh;
        }

        private void OnCallbackRefresh(SyncList<uint>.Operation operation, int itemindex, uint olditem, uint newitem)
        {
            Debug.Log($"Refresh on PlayerBase. Op {operation}, index {itemindex}, old {olditem}, new {newitem}");

            Refresh();
        }

        private void Refresh() =>
            StartCoroutine(RefreshRoutine());

        private IEnumerator RefreshRoutine()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            var tryCount = 0;
            while (!_gameMode.CurrentPlayersBaseList.All(netId => NetworkClient.spawned.ContainsKey(netId)))
            {
                yield return null;
                tryCount++;
                if (tryCount > 5)
                {
                    Debug.LogWarning("Failed to refresh UI");
                    yield break;
                }

                Debug.Log("Checking again..");
            }


            Debug.Log($"Refresh! List length is {_gameMode.CurrentPlayersBaseList.Count}");
            foreach (uint playerId in _gameMode.CurrentPlayersBaseList)
            {
                PlayerBase player = NetworkClient.spawned[playerId].GetComponent<PlayerBase>();

                ScoreLine line = Instantiate(_linePrefab, transform);
                line.Init(player.name, player.Score.ToString());
                // line.Init(player.name, "11");
            }
        }
    }
}