using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LoadingBarUI : MonoBehaviour
    {
        [Header("Настройки загрузки")]
        [Tooltip("Image с типом Fill, который будет заполняться")]
        [SerializeField] private Image fillImage;

        [Tooltip("Продолжительность анимации заполнения (в секундах)")]
        [SerializeField] private float fillDuration = 1.0f;

        private void Awake()
        {
            if (fillImage == null)
            {
                Debug.LogError("LoadingBarUI: fillImage не назначен!");
                return;
            }

            fillImage.fillAmount = 0f;
        }

        private void Start()
        {
            AnimateFill();
        }

        private void AnimateFill()
        {
            fillImage.DOFillAmount(1f, fillDuration).SetEase(Ease.InOutSine);
        }
    }
}
