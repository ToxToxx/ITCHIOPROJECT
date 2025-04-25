using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ItemGridManager : MonoBehaviour
    {
        private void Start()
        {
            UpdateUIGrid();
        }

        private void UpdateUIGrid()
        {
            int completedLevels = CountCompletedLevels();
            int totalItems = transform.childCount;

            for (int i = 0; i < totalItems; i++)
            {
                GameObject itemIcon = transform.GetChild(i).gameObject;

                if (itemIcon.TryGetComponent<Button>(out var button))
                {
                    bool isUnlocked = i < completedLevels;
                    button.interactable = isUnlocked;

                    Color color = isUnlocked ? Color.white : Color.grey;
                    foreach (var image in itemIcon.GetComponentsInChildren<Image>())
                    {
                        image.color = color;
                    }
                }
            }
        }

        private int CountCompletedLevels()
        {
            int count = 0;
            int totalLevels = Loader.LevelCount;

            for (int i = 1; i <= totalLevels; i++)
            {
                if (LevelManager.Instance != null && LevelManager.Instance.IsLevelCompleted(i))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
