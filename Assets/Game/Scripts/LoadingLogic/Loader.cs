using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [ObfuscationAttribute(Exclude = true)]
    public class Loader : MonoBehaviour
    {
        public enum Scene
        {
            MainMenu,
            LoadingScreen,
            Level1,
            Level2,
            Level3,
            Level4,
        }

        public const int LevelCount = 4;
        private static Scene _targetScene;

        public static int GetLevelNumber(Scene scene)
        {
            if (scene >= Scene.Level1 && scene <= Scene.Level4)
                return (int)scene - (int)Scene.Level1 + 1;

            return -1;
        }

        public static void Load(Scene target)
        {
            _targetScene = target;
            SceneManager.LoadScene(Scene.LoadingScreen.ToString());
        }

        public static void LoaderCallback()
        {
            SceneManager.LoadScene(_targetScene.ToString());
        }

    }
}
