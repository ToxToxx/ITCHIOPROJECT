using UnityEngine;
using UniRx;

namespace Game
{
    public class GameOverManager : IInitializable
    {
        private readonly GameObject _gameOverCanvas;
        private GameStateManager _gameStateManager;
        private bool _isGameOver;

        public GameOverManager(GameObject gameOverCanvas)
        {
            _gameOverCanvas = gameOverCanvas;
        }

        public void InjectDependencies(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public void Initialize()
        {
            _gameStateManager.CurrentState
                .Subscribe(state =>
                {
                    if (state == GameState.Result) TriggerGameOver();
                })
                .AddTo(_gameOverCanvas);
        }

        private void TriggerGameOver()
        {
            if (_isGameOver) return;
            _isGameOver = true;

            _gameOverCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
