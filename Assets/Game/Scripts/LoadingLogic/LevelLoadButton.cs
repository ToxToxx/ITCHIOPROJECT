using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [ObfuscationAttribute(Exclude = true)]
    public class LevelLoadButton : MonoBehaviour
    {

        [SerializeField] private Button _button;
        [SerializeField] private Loader.Scene _targetScene;

        private void Start()
        {
            int levelNumber = Loader.GetLevelNumber(_targetScene);

            if (levelNumber == -1)
            {
                _button.interactable = true;
                return;
            }

            bool isUnlocked = LevelManager.Instance.IsLevelUnlocked(levelNumber);
            _button.interactable = isUnlocked;

            if (!isUnlocked)
            {
                ColorBlock colors = _button.colors;
                _button.colors = colors;
            }

            _button.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                Loader.Load(_targetScene);
            });
        }

    }
}

