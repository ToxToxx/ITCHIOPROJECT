using System;
using UnityEngine;

namespace Game
{
    public class GoalManager : MonoBehaviour
    {
        public static GoalManager Instance { get; private set; }

        public int CurrentScore { get; private set; }
        public int BestScore { get; private set; }

        private float _pointsPerSecond = 1f;
        private float _timer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            LoadBestScore();
        }

        private void Update()
        {
            if (GameStateManager.Instance.CurrentState.Value == GameState.Playing)
            {
                _timer += Time.deltaTime;

                if (_timer >= 1f)
                {
                    _timer -= 1f;
                    AddPoints(Mathf.RoundToInt(_pointsPerSecond));
                }
            }
        }

        private void AddPoints(int points)
        {
            CurrentScore += points;
            Debug.Log($"Очки: {CurrentScore}");

            if (CurrentScore > BestScore)
            {
                BestScore = CurrentScore;
                SaveBestScore();
                Debug.Log($"Новый лучший результат: {BestScore}");
            }
        }

        private void SaveBestScore()
        {
            PlayerPrefs.SetInt("BestScore", BestScore);
            PlayerPrefs.Save();
        }

        private void LoadBestScore()
        {
            BestScore = PlayerPrefs.GetInt("BestScore", 0);
        }

        public void ResetScore()
        {
            CurrentScore = 0;
            _timer = 0;
        }

        // Вот добавленный метод настройки
        public void SetGoalSettings(float pointsPerSecond)
        {
            _pointsPerSecond = pointsPerSecond;
        }
    }
}
