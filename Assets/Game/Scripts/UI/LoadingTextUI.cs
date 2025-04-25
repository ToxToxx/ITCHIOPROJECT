using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Game
{
    public class LoadingTextUI : MonoBehaviour
    {
        [Header("Настройки текста")]
        [SerializeField] private TextMeshProUGUI loadingText;  // Текст "Loading..."
        [SerializeField] private float animationSpeed = 0.5f;   // Скорость смены точек

        private int dotCount = 0;
        private string baseText = "Loading";

        private void Start()
        {
            if (loadingText == null)
            {
                Debug.LogError("LoadingTextUI: Не назначен TextMeshProUGUI!");
                return;
            }

            StartLoadingAnimation();
        }

        private void StartLoadingAnimation()
        {
            DOTween.Sequence()
                .AppendCallback(UpdateLoadingText)
                .AppendInterval(animationSpeed)
                .SetLoops(-1); // Зацикленный эффект
        }

        private void UpdateLoadingText()
        {
            dotCount = (dotCount + 1) % 4; // Цикл от 0 до 3 точек
            loadingText.text = baseText + new string('.', dotCount);
        }
    }
}
