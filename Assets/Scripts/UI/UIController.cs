using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private EndGameScreen _endGameScreen;
        [SerializeField] private GameMode _gameMode;

        private void Start()
        {
            _gameMode.GameEnded += HandleGameEnd;
            _gameMode.GameRestart += HandleGameStart;
        }

        private void OnDestroy()
        {
            _gameMode.GameEnded -= HandleGameEnd;
            _gameMode.GameRestart -= HandleGameStart;
        }

        private void HandleGameStart() =>
            _endGameScreen.gameObject.SetActive(false);

        private void HandleGameEnd(string winnerName)
        {
            _endGameScreen.SetWinnerName(winnerName);
            _endGameScreen.gameObject.SetActive(true);
        }
    }
}