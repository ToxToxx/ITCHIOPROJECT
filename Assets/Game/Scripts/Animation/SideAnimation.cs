using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SideAnimation : MonoBehaviour
    {
        [Header("Настройки анимации")]
        [SerializeField] private RectTransform targetElement;
        [SerializeField] private float sideOffset = 500f;
        [SerializeField] private float animationDuration = 1.0f;
        [SerializeField] private float startDelay = 0.5f;
        [SerializeField] private Ease animationEase = Ease.OutExpo;

        [Header("Дополнительные эффекты")]
        [SerializeField] private bool fadeIn = true;
        [SerializeField] private float fadeDuration = 0.5f;

        private Vector2 _originalPosition;

        private void Start()
        {
            if (targetElement == null)
            {
                Debug.LogWarning("SideAnimation: Не назначен targetElement!");
                return;
            }

            _originalPosition = targetElement.anchoredPosition;
            targetElement.anchoredPosition = _originalPosition + new Vector2(sideOffset, 0f);

            if (fadeIn && targetElement.TryGetComponent<Image>(out var image))
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
            }

            Invoke(nameof(PlayAnimation), startDelay);
        }

        private void PlayAnimation()
        {

            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                targetElement.DOAnchorPos(_originalPosition, animationDuration)
                             .SetEase(animationEase)
            );

            if (fadeIn && targetElement.TryGetComponent<Image>(out var image))
            {
                sequence.Join(
                    image.DOFade(1f, fadeDuration)
                );
            }
        }
    }
}
