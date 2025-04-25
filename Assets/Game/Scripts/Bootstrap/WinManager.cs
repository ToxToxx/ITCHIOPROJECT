using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

namespace Game
{
    public class WinManager : IInitializable
    {
        private readonly GameObject _winCanvas;
        private readonly int _reward;
        private readonly int _levelIndex;

        private GameStateManager _gameStateManager;
        private LevelManager _levelManager;
        private bool _isGameWon;

        public WinManager(GameObject winCanvas, int reward, int levelIndex)
        {
            _winCanvas = winCanvas;
            _reward = reward;
            _levelIndex = levelIndex;
        }

        public void InjectDependencies(
            GameStateManager gameStateManager,
            LevelManager levelManager)
        {
            _gameStateManager = gameStateManager;
            _levelManager = levelManager;
        }

        public void Initialize()
        {
            _gameStateManager.CurrentState
                .Subscribe(state =>
                {
                    if (state == GameState.Win) GameWin();
                })
                .AddTo(_winCanvas);
        }

        private void GameWin()
        {
            if (_isGameWon) return;
            _isGameWon = true;

            if (_levelManager != null)
            {
                _levelManager.CompleteLevel(_levelIndex);
            }
            _winCanvas.SetActive(true);
        }
    }
}
