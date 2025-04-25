using UnityEngine;
using UniRx;

namespace Game
{
    public interface IInitializable
    {
        void Initialize();
    }

    public class InGameManagersBootstrap : MonoBehaviour
    {
        [Header("Win Settings")]
        private GameObject _winCanvas;
        private int _reward = 0;
        private int _currentLevelIndex = 1;

        [Header("GameOver Settings")]
        private GameObject _gameOverCanvas;

        private GameStateManager _gameStateManager;
        private WinManager _winManager;
        private GameOverManager _gameOverManager;
        private bool _isInitialized = false;

        private void Awake()
        {
            GameEventSystem.OnGameStateChanged
                .Subscribe(state => HandleGameState(state))
                .AddTo(this);
        }

        public void InitializeManagers()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("InGameManagersBootstrap is already initialized!");
                return;
            }

            // Проверяем, что все поля установлены
            if (_winCanvas == null || _gameOverCanvas == null)
            {
                Debug.LogError("WinCanvas or GameOverCanvas is not set in InGameManagersBootstrap!");
                return;
            }

            var levelManager = LevelManager.Instance;

            _gameStateManager = new GameStateManager();
            _winManager = new WinManager(_winCanvas, _reward, _currentLevelIndex);
            _gameOverManager = new GameOverManager(_gameOverCanvas);

            _winManager.InjectDependencies(_gameStateManager, levelManager);
            _gameOverManager.InjectDependencies(_gameStateManager);

            _gameStateManager.Initialize();
            _winManager.Initialize();
            _gameOverManager.Initialize();

            _gameStateManager.SetState(GameState.Playing);

            _isInitialized = true;
        }

        private void HandleGameState(GameState state)
        {
            Time.timeScale = state == GameState.Paused ? 0f : 1f;
        }

        public void SetWinSettings(GameObject winCanvas, int reward, int currentLevelIndex)
        {
            _winCanvas = winCanvas;
            _reward = reward;
            _currentLevelIndex = currentLevelIndex;
        }

        public void SetGameOverSettings(GameObject gameOverCanvas)
        {
            _gameOverCanvas = gameOverCanvas;
        }
    }
}