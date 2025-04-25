using System.Reflection;

namespace Game
{
    [ObfuscationAttribute(Exclude = true)]
    public static class SceneExtensions
    {
        public static string GetSceneName(Loader.Scene scene)
        {
            switch (scene)
            {
                case Loader.Scene.MainMenu: return "MainMenu";
                case Loader.Scene.LoadingScreen: return "LoadingScreen";
                case Loader.Scene.Level1: return "Level1";
                case Loader.Scene.Level2: return "Level2";
                case Loader.Scene.Level3: return "Level3";
                case Loader.Scene.Level4: return "Level4";
                default: return string.Empty;
            }
        }
    }
}

