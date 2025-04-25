using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class TutorialUISlider : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject[] _slides;
        [SerializeField] private Button _nextButton;

        private int _currentSlideIndex = 0;

        private void Start()
        {
            if (_slides == null || _slides.Length == 0)
            {
                Debug.LogError("Слайды обучения не настроены!");
                return;
            }

            for (int i = 0; i < _slides.Length; i++)
            {
                _slides[i].SetActive(false);
            }

            ShowSlide(_currentSlideIndex);
            _nextButton.onClick.AddListener(ShowNextSlide);
        }

        private void ShowSlide(int index)
        {
            for (int i = 0; i < _slides.Length; i++)
            {
                if (i != index)
                    _slides[i].SetActive(false);
            }

            GameObject activeSlide = _slides[index];
            activeSlide.SetActive(true);
            activeSlide.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            if (activeSlide.TryGetComponent<Image>(out var img))
            {
                Color targetColor = img.color;
                targetColor.a = 1f;
                Color startColor = img.color;
                startColor.a = 0f;
                img.color = startColor;
                img.DOFade(1f, 0.5f);
            }

            activeSlide.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }

        private void ShowNextSlide()
        {
            _currentSlideIndex = (_currentSlideIndex + 1) % _slides.Length;
            ShowSlide(_currentSlideIndex);
        }
    }
}
