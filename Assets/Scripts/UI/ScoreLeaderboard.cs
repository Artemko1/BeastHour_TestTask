using Mirror;
using UnityEngine;

namespace UI
{
    public class ScoreLeaderboard : MonoBehaviour
    {
        [SerializeField] private ScoreLine _linePrefab;
        [SerializeField] private GameMode _gameMode;
        [SerializeField] private NetManager _networkManager;

        private void Start()
        {
            Refresh();
            _gameMode.CurrentPlayersList.Callback += Refresh;
        }

        private void OnEnable()
        {
            // _gameMode.GameRestart += Refresh;
            // _gameMode.ClientPlayerScoreChanged += Refresh;

            // _networkManager.ServerPlayerConnected += Refresh;
            // _networkManager.ServerPlayerDisconnected += Refresh;
            // _networkManager.ClientConnected += Refresh;
        }

        private void OnDisable()
        {
            // _gameMode.GameRestart -= Refresh;
            // _gameMode.ClientPlayerScoreChanged -= Refresh;
            // _networkManager.ServerPlayerConnected -= Refresh;
            // _networkManager.ServerPlayerDisconnected -= Refresh;
        }

        private void Refresh(SyncList<PlayerBase>.Operation op, int itemindex, PlayerBase olditem,
            PlayerBase newitem) =>
            Refresh();

        private void Refresh(PlayerBase player) => Refresh();

        private void Refresh()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            Debug.Log($"List length is {_gameMode.CurrentPlayersList.Count}");
            for (var i = 0; i < _gameMode.CurrentPlayersList.Count; i++)
            {
                PlayerBase playerBase = _gameMode.CurrentPlayersList[i];
                if (playerBase == null)
                {
                    Debug.LogWarning("Null object in syncList");
                    continue;
                }

                ScoreLine line = Instantiate(_linePrefab, transform);
                line.Init(playerBase.Name, playerBase.Score.ToString());
            }
        }
    }
}