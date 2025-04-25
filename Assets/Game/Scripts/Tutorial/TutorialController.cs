using UnityEngine;
using UnityEngine.UI;


namespace Game
{
    public class TutorialController : MonoBehaviour
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

            ShowSlide(_currentSlideIndex);

            _nextButton.onClick.AddListener(ShowNextSlide);

            Time.timeScale = 0f;
            GameStateManager.Instance.SetState(GameState.Paused);
        }

        private void ShowSlide(int index)
        {
            for (int i = 0; i < _slides.Length; i++)
            {
                _slides[i].SetActive(i == index);
            }
        }

        private void ShowNextSlide()
        {
            _currentSlideIndex++;

            if (_currentSlideIndex >= _slides.Length)
            {
                EndTutorial();
                return;
            }

            ShowSlide(_currentSlideIndex);
        }

        private void EndTutorial()
        {
            gameObject.SetActive(false);

            Time.timeScale = 1f;
            GameStateManager.Instance.SetState(GameState.Playing);
        }

    }
}
