using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

namespace Game
{
    public class TimerUI : MonoBehaviour
    {
        [Header("Timer Settings")]
        [SerializeField] private float _startingTime = 60f;
        [SerializeField] private TextMeshProUGUI _timerText;

        private float _currentTime;
        private bool _isTimerRunning = false;

        private void Awake()
        {
            _currentTime = _startingTime;
            UpdateTimerUI();
        }
        private void Start()
        {
            HandleGameState(GameStateManager.Instance.CurrentState.Value);
        }


        private void OnEnable()
        {
            GameEventSystem.OnGameStateChanged
                .Subscribe(state => HandleGameState(state))
                .AddTo(this);
        }

        private void Update()
        {
            if (GameStateManager.Instance.CurrentState.Value != GameState.Playing)
                return;

            if (_isTimerRunning)
            {
                _currentTime -= Time.deltaTime;

                if (_currentTime <= 0f)
                {
                    _currentTime = 0f;
                    GameStateManager.Instance.SetState(GameState.GameOver);
                }
                UpdateTimerUI();
            }
        }

        private void HandleGameState(GameState state)
        {
            if (state == GameState.Playing)
            {
                StartTimer();
            }
            else
            {
                StopTimer();
            }
        }

        private void StartTimer()
        {
            _isTimerRunning = true;
        }

        private void StopTimer()
        {
            _isTimerRunning = false;
        }

        private void UpdateTimerUI()
        {
            int minutes = Mathf.FloorToInt(_currentTime / 60f);
            int seconds = Mathf.FloorToInt(_currentTime % 60f);
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
