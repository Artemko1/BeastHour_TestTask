using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private EndGameScreen _endGameScreen;

        [SerializeField] private GameMode _gameMode;
        // private EndGameScreen _endGameScreen;

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
            // Debug.Log($"Destroying {_endGameScreen}", _endGameScreen);
            // Destroy(_endGameScreen);
            _endGameScreen.gameObject.SetActive(false);

        private void HandleGameEnd(string winnerName)
        {
            // Debug.Log("Creating", _endGameScreen);
            // _endGameScreen = Instantiate(_endGameScreenPrefab, transform);
            _endGameScreen.SetWinnerName(winnerName);
            _endGameScreen.gameObject.SetActive(true);
        }
    }
}