using UniRx;

namespace Game
{
    public static class GameEventSystem
    {
        public static readonly ReactiveCommand<GameState> OnGameStateChanged = new ReactiveCommand<GameState>();
        public static readonly Subject<Unit> OnPlayerCrashed = new Subject<Unit>();
    }
}