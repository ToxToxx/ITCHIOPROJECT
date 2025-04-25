using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ResumeButton : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseCanvas;
        [SerializeField] private Button _resumeButton;

        private void Start()
        {
            _resumeButton.onClick.AddListener(ResumeGame);
        }

        private void ResumeGame()
        {
            Time.timeScale = 1f;
            _pauseCanvas.SetActive(false);
            GameStateManager.Instance.SetState(GameState.Playing);
        }
    }
}
