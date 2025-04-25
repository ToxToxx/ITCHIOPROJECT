using UniRx;

namespace Game
{
    public enum GameState
    {
        Playing,
        GameOver,
        Win,
        Paused
    }

    public class GameStateManager : IInitializable
    {
        private ReactiveProperty<GameState> _currentState = new ReactiveProperty<GameState>(GameState.Paused);
        public IReadOnlyReactiveProperty<GameState> CurrentState => _currentState;

        public static GameStateManager Instance { get; private set; }

        public static bool IsPlaying => Instance != null && Instance._currentState.Value == GameState.Playing;

        public void Initialize()
        {
            Instance = this;
            _currentState.Value = GameState.Playing;
        }

        public void SetState(GameState newState)
        {
            if (_currentState.Value == newState) return;

            _currentState.Value = newState;
            GameEventSystem.OnGameStateChanged.Execute(newState);
        }
    }
}
