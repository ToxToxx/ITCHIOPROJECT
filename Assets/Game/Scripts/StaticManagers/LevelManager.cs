using System.Reflection;
using UnityEngine;

namespace Game
{
    [ObfuscationAttribute(Exclude = true)]
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [SerializeField] private bool[] _levelsCompleted;
        [SerializeField] private bool[] _levelsUnlocked;
        [SerializeField] private bool resetAllLevels = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeLevels();
                LoadProgress();

                if (resetAllLevels)
                {
                    ResetProgress();
                    resetAllLevels = false;
                    SaveProgress();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeLevels()
        {
            _levelsCompleted = new bool[Loader.LevelCount];
            _levelsUnlocked = new bool[Loader.LevelCount];

            if (PlayerPrefs.GetInt("FirstRun", 1) == 1)
            {
                PlayerPrefs.SetInt("FirstRun", 0);

                _levelsUnlocked[0] = true;
                SaveProgress();
            }
        }

        public bool IsLevelUnlocked(int levelNumber)
        {
            if (levelNumber < 1 || levelNumber > Loader.LevelCount)
                return false;

            return _levelsUnlocked[levelNumber - 1];
        }

        public bool IsLevelCompleted(int levelNumber)
        {
            if (levelNumber < 1 || levelNumber > Loader.LevelCount)
                return false;

            return _levelsCompleted[levelNumber - 1];
        }

        public void CompleteLevel(int levelNumber)
        {
            if (levelNumber < 1 || levelNumber > Loader.LevelCount) return;

            _levelsCompleted[levelNumber - 1] = true;

            if (levelNumber < Loader.LevelCount)
                _levelsUnlocked[levelNumber] = true;

            SaveProgress();
        }

        private void SaveProgress()
        {
            for (int i = 0; i < Loader.LevelCount; i++)
            {
                PlayerPrefs.SetInt($"Level_Completed_{i + 1}", _levelsCompleted[i] ? 1 : 0);
                PlayerPrefs.SetInt($"Level_Unlocked_{i + 1}", _levelsUnlocked[i] ? 1 : 0);
            }

            PlayerPrefs.Save();
        }

        private void LoadProgress()
        {
            for (int i = 0; i < Loader.LevelCount; i++)
            {
                _levelsCompleted[i] = PlayerPrefs.GetInt($"Level_Completed_{i + 1}", 0) == 1;
                _levelsUnlocked[i] = PlayerPrefs.GetInt($"Level_Unlocked_{i + 1}", 0) == 1;
            }
        }

        public void ResetProgress()
        {
            for (int i = 0; i < Loader.LevelCount; i++)
            {
                _levelsCompleted[i] = false;
                _levelsUnlocked[i] = false;
            }


            _levelsUnlocked[0] = true;

            for (int i = 0; i < Loader.LevelCount; i++)
            {
                PlayerPrefs.DeleteKey($"Level_Completed_{i + 1}");
                PlayerPrefs.DeleteKey($"Level_Unlocked_{i + 1}");
            }

            PlayerPrefs.SetInt("FirstRun", 1);
            PlayerPrefs.Save();

            Debug.Log("Прогресс сброшен!");
        }
    }



}
