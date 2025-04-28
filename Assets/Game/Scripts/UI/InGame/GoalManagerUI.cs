using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GoalManagerUI : MonoBehaviour
    {
        public static GoalManagerUI Instance { get; private set; }

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;

        private void Start()
        {
            // При старте обновляем UI начальными данными
            UpdateScoreUI(GoalManager.Instance.CurrentScore);
            UpdateBestScoreUI(GoalManager.Instance.BestScore);
        }

        private void Update()
        {
            // Можно обновлять каждую frame, но правильнее будет делать отдельное событие при изменении счета
            UpdateScoreUI(GoalManager.Instance.CurrentScore);
            UpdateBestScoreUI(GoalManager.Instance.BestScore);
        }

        private void UpdateScoreUI(int score)
        {
            if (_currentScoreText != null)
            {
                _currentScoreText.text = $"Score: {score}";
            }
        }

        private void UpdateBestScoreUI(int bestScore)
        {
            if (_bestScoreText != null)
            {
                _bestScoreText.text = $"Best: {bestScore}";
            }
        }
    }
}
