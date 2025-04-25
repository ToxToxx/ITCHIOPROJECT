using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Game
{
    public class CanvasPopUp : MonoBehaviour
    {
        [SerializeField] private float _popupFadeDuration = 0.5f; // Время анимации появления/исчезновения

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                Debug.LogError("CanvasGroup отсутствует на объекте!", this);
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            // Делаем объект кликабельным
            if (!TryGetComponent<Button>(out var button))
            {
                button = gameObject.AddComponent<Button>();
            }
            button.onClick.AddListener(ClosePopup);
        }

        private void OnEnable()
        {
            if (_canvasGroup == null) return;

            // Останавливаем текущие анимации, если есть
            DOTween.Kill(_canvasGroup);

            // Устанавливаем полную видимость
            _canvasGroup.alpha = 0f; // Стартуем с 0, чтобы анимация сработала
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            // Анимация появления (масштаб и прозрачность)
            transform.localScale = Vector3.zero;
            transform.DOScale(1f, _popupFadeDuration).SetEase(Ease.OutBack);
            _canvasGroup.DOFade(1f, _popupFadeDuration);
        }

        private void OnDisable()
        {
            if (_canvasGroup != null)
            {
                DOTween.Kill(_canvasGroup);
                _canvasGroup.alpha = 0f;
            }
        }

        private void ClosePopup()
        {
            if (_canvasGroup == null) return;

            // Анимация скрытия
            transform.DOScale(0f, _popupFadeDuration).SetEase(Ease.InBack);
            _canvasGroup.DOFade(0f, _popupFadeDuration)
                .OnComplete(() =>
                {
                    _canvasGroup.interactable = false;
                    _canvasGroup.blocksRaycasts = false;
                    // Не отключаем объект, просто оставляем alpha = 0
                });
        }
    }
}