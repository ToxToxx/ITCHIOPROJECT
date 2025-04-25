using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameOverAnimation : MonoBehaviour
    {
        [Header("UI Элементы")]
        [SerializeField] private RectTransform[] _uiElements;

        [Header("Эффект появления с падением")]
        [SerializeField] private float _fallDistance = 100f;
        [SerializeField] private float _fallDuration = 1f;
        [SerializeField] private Ease _fallEase = Ease.OutBounce;

        [Header("Эффект дрожи (тревожность)")]
        [SerializeField] private float _shakeDuration = 0.5f;
        [SerializeField] private Vector3 _shakeStrength = new Vector3(5f, 5f, 0f);
        [SerializeField] private int _shakeVibrato = 10;

        [Header("Эффект затемнения")]
        [SerializeField] private float _fadeDuration = 1f;

        private void OnEnable()
        {
            if (_uiElements == null || _uiElements.Length == 0)
            {
                Debug.LogWarning("Нет назначенных UI-элементов для анимации GameOverAnimation.");
                return;
            }

            foreach (var element in _uiElements)
            {
                element.anchoredPosition += new Vector2(0, _fallDistance);
                element.gameObject.SetActive(false);
                if (element.TryGetComponent<Image>(out Image img))
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
                }
            }

            PlayGameOverAnimation();
        }

        private void PlayGameOverAnimation()
        {
            Sequence mainSequence = DOTween.Sequence();

            for (int i = 0; i < _uiElements.Length; i++)
            {
                RectTransform element = _uiElements[i];
                float delay = i * 0.1f;
                Vector2 startPos = element.anchoredPosition;

                element.gameObject.SetActive(true);

                // Падение вниз с эффектом
                mainSequence.Insert(delay, element.DOAnchorPosY(startPos.y - _fallDistance, _fallDuration)
                    .SetEase(_fallEase));

                // Лёгкая дрожь
                mainSequence.Insert(delay + _fallDuration * 0.5f, element.DOShakeAnchorPos(_shakeDuration, _shakeStrength, _shakeVibrato));

                // Постепенное появление (если есть компонент Image)
                if (element.TryGetComponent<Image>(out Image img))
                {
                    mainSequence.Insert(delay, img.DOFade(1f, _fadeDuration));
                }
            }
        }
    }
}
