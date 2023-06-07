using UI;
using UnityEngine;

public class ScoreLeaderboardController : MonoBehaviour, IAnyScoreListener
    {
        [SerializeField] private ScoreLine _linePrefab;
        private Coroutine _refreshRoutine;

        private void Start()
        {
            var listener = Contexts.sharedInstance.game.CreateEntity();
            listener.AddAnyScoreListener(this);
            Refresh();
        }

        public void OnAnyScore(GameEntity entity, int value)
        {
            Refresh();
        }

        private void Refresh()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.Score).GetEntities();

            foreach (var e in entities)
            {
                int score = e.score.Value;

                ScoreLine line = Instantiate(_linePrefab, transform);
                line.Init(e.name.Value, score.ToString());
            }
        }
    }