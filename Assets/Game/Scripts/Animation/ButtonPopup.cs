using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Game
{
    public class ButtonPopup : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _popupPrefab; // Префаб попапа
        [SerializeField] private Transform _targetCanvas; // Куда инстанциировать попап
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private Vector3 _popupScale = new(1.2f, 1.2f, 1.2f);

        [Header("Сообщение попапа")]
        [SerializeField] private string message = "Привет!"; // Текст сообщения
        [SerializeField] private float _textTypingSpeed = 0.05f; // Скорость печати текста (секунд на символ)

        private GameObject _currentPopup;
        private Vector3 _originalScale;
        private bool _isPopupVisible = false;

        private void Awake()
        {
            _button.onClick.AddListener(TogglePopup);
        }

        private void TogglePopup()
        {
            if (_popupPrefab == null || _targetCanvas == null || !_button.interactable) return;

            if (_isPopupVisible)
            {
                HidePopup();
            }
            else
            {
                ShowPopup();
            }

            _isPopupVisible = !_isPopupVisible;
        }

        private void ShowPopup()
        {
            _currentPopup = Instantiate(_popupPrefab, _targetCanvas);
            _originalScale = _currentPopup.transform.localScale;
            _currentPopup.transform.localScale = Vector3.zero; // Стартовый нулевой размер

            // Находим компонент TextMeshProUGUI в дочернем объекте
            var textComponent = _currentPopup.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                AnimateText(textComponent); // Запускаем анимацию текста
            }

            _currentPopup.transform.DOScale(_popupScale, _animationDuration)
                .SetEase(Ease.OutBack);

            // Добавляем обработку закрытия по клику
            if (!_currentPopup.TryGetComponent<Button>(out var popupButton))
            {
                popupButton = _currentPopup.AddComponent<Button>();
                if (_currentPopup.TryGetComponent<Image>(out var img))
                {
                    popupButton.targetGraphic = img;
                }
            }
            popupButton.onClick.AddListener(HidePopup);
        }

        private void AnimateText(TextMeshProUGUI textComponent)
        {
            textComponent.text = ""; // Очищаем текст перед анимацией
            string fullText = message;
            DOTween.To(() => 0, x =>
            {
                int charsToShow = Mathf.RoundToInt(x);
                textComponent.text = fullText.Substring(0, Mathf.Min(charsToShow, fullText.Length));
            }, fullText.Length, fullText.Length * _textTypingSpeed)
            .SetEase(Ease.Linear);
        }

        private void HidePopup()
        {
            if (_currentPopup == null) return;

            _currentPopup.transform.DOScale(Vector3.zero, _animationDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    Destroy(_currentPopup);
                    _isPopupVisible = false;
                });
        }
    }
}
