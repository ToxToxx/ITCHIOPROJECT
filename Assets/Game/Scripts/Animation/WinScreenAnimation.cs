using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class WinScreenAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _uiElements;

        [Header("Анимационные параметры")]
        [SerializeField] private float _flyInDuration = 1f;
        [SerializeField] private float _popDuration = 0.6f;
        [SerializeField] private float _flutterAmount = 10f; // Колебание по X
        [SerializeField] private float _flutterDuration = 0.3f;
        [SerializeField] private float _pulseScale = 1.05f;
        [SerializeField] private float _pulseDuration = 1f;

        private void OnEnable()
        {
            PlayWinAnimation();
        }

        private void PlayWinAnimation()
        {
            if (_uiElements == null || _uiElements.Length == 0)
            {
                Debug.LogWarning("Нет UI-элементов для анимации.");
                return;
            }

            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < _uiElements.Length; i++)
            {
                RectTransform element = _uiElements[i];
                float delay = i * 0.2f;

                element.anchoredPosition += new Vector2(0, -500);
                element.localScale = Vector3.zero;

                sequence.Insert(delay, element.DOAnchorPosY(element.anchoredPosition.y + 500, _flyInDuration).SetEase(Ease.OutCubic));
                sequence.Insert(delay, element.DOScale(1f, _popDuration).SetEase(Ease.OutBack));
                sequence.Insert(delay + 0.5f, element.DOAnchorPosX(element.anchoredPosition.x + _flutterAmount, _flutterDuration)
                    .SetEase(Ease.InOutSine).SetLoops(4, LoopType.Yoyo));
                sequence.Insert(delay + 0.7f, element.DOScale(_pulseScale, _pulseDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo));
            }
        }
    }
}