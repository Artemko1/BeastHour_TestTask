using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private Text _winnerNameText;

        public void SetWinnerName(string winnerName) => _winnerNameText.text = "Winner is: " + winnerName;
    }
}