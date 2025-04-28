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

        [Header("GameOver Settings")]
        private GameObject _gameOverCanvas;

        private GameStateManager _gameStateManager;
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
            if ( _gameOverCanvas == null)
            {
                Debug.LogError("WinCanvas or GameOverCanvas is not set in InGameManagersBootstrap!");
                return;
            }

            _gameStateManager = new GameStateManager();
            _gameOverManager = new GameOverManager(_gameOverCanvas);

            _gameOverManager.InjectDependencies(_gameStateManager);

            _gameStateManager.Initialize();
            _gameOverManager.Initialize();

            _gameStateManager.SetState(GameState.Playing);

            _isInitialized = true;
        }

        private void HandleGameState(GameState state)
        {
            Time.timeScale = state == GameState.Paused ? 0f : 1f;
        }


        public void SetGameOverSettings(GameObject gameOverCanvas)
        {
            _gameOverCanvas = gameOverCanvas;
        }
    }
}