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
            Refresh();
            _gameMode.CurrentPlayersList.Callback += Refresh;
            _gameMode.ClientPlayerScoreChanged += Refresh;
            // _gameMode.GameRestart += Refresh;
        }

        private void Refresh(SyncList<PlayerInfo>.Operation op, int itemindex, PlayerInfo olditem,
            PlayerInfo newitem)
        {
            Debug.Log($"Refresh on ListSync callback. Op {op}, index {itemindex}, old {olditem}, new {newitem}");
            Refresh();
        }

        private void Refresh()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            Debug.Log($"Refresh! List length is {_gameMode.CurrentPlayersList.Count}");
            for (var i = 0; i < _gameMode.CurrentPlayersList.Count; i++)
            {
                PlayerInfo player = _gameMode.CurrentPlayersList[i];
                if (player == null)
                {
                    Debug.LogWarning("Null object in syncList");
                    continue;
                }

                ScoreLine line = Instantiate(_linePrefab, transform);
                line.Init(player.Name, player.Score.ToString());
            }
        }
    }
}