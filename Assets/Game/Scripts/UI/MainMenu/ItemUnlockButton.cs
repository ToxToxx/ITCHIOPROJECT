using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class ItemUnlockButton : MonoBehaviour
    {
        [SerializeField] private Button _fruitButton;
        [SerializeField] private GameObject _popupPrefab; // Префаб попапа
        [SerializeField] private Transform _popupParent; // Контейнер попапов
        [SerializeField] private string _message;
        [SerializeField] private float _delayedCall = 3;
        [SerializeField] private float _popupFadeDuration = 0.5f; // Время анимации попапа

        private bool _isPopupActive = false; // Флаг блокировки повторного вызова

        private void Start()
        {
            _fruitButton.onClick.AddListener(OnButtonClick);
        }

        public void OnButtonClick()
        {
            if (_isPopupActive) return; // Если попап уже активен, не создаем новый

            _isPopupActive = true; // Блокируем повторные вызовы
            ShowPopup(_message);
        }

        private void ShowPopup(string message)
        {
            GameObject popupInstance = Instantiate(_popupPrefab, _popupParent);
            popupInstance.transform.localScale = Vector3.zero; // Начальный размер
            popupInstance.GetComponent<CanvasGroup>().alpha = 0f; // Начальная прозрачность
            popupInstance.GetComponentInChildren<TextMeshProUGUI>().text = message;

            // Анимация появления попапа
            popupInstance.transform.DOScale(1f, _popupFadeDuration).SetEase(Ease.OutBack);
            popupInstance.GetComponent<CanvasGroup>().DOFade(1f, _popupFadeDuration);

            // Анимация исчезновения попапа через _delayedCall сек.
            DOVirtual.DelayedCall(_delayedCall, () =>
            {
                popupInstance.transform.DOScale(0f, _popupFadeDuration).SetEase(Ease.InBack);
                popupInstance.GetComponent<CanvasGroup>().DOFade(0f, _popupFadeDuration)
                    .OnComplete(() =>
                    {
                        Destroy(popupInstance);
                        _isPopupActive = false; // Разблокируем повторный вызов
                    });
            });
        }
    }
}
