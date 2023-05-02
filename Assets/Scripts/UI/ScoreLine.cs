using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScoreLine : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _scoreText;

        public void Init(string playerName, string playerScore)
        {
            _nameText.text = playerName;
            _scoreText.text = playerScore;
        }
    }
}