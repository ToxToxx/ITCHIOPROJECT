using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ImageSlider : MonoBehaviour
    {
        [Header("Настройки слайдера")]
        [SerializeField] private GameObject[] images; // Массив изображений в контейнере
        [SerializeField] private Button leftButton;   // Кнопка для переключения влево
        [SerializeField] private Button rightButton;  // Кнопка для переключения вправо

        private int currentIndex = 0;

        private void Awake()
        {
            // Назначаем обработчики кнопок
            leftButton.onClick.AddListener(SlideLeft);
            rightButton.onClick.AddListener(SlideRight);

            UpdateSlider();
        }

        private void SlideLeft()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateSlider();
            }
        }

        private void SlideRight()
        {
            if (currentIndex < images.Length - 1)
            {
                currentIndex++;
                UpdateSlider();
            }
        }

        private void UpdateSlider()
        {
            // Активируем только текущее изображение, остальные отключаем
            for (int i = 0; i < images.Length; i++)
            {
                images[i].SetActive(i == currentIndex);
            }

            // Обновляем состояние кнопок
            leftButton.interactable = currentIndex > 0;
            rightButton.interactable = currentIndex < images.Length - 1;
        }
    }
}
