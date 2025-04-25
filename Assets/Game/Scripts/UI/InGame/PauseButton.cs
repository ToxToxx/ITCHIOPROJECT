using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseCanvas;
        [SerializeField] private Button _pauseButton;

        private void Start()
        {
            _pauseCanvas.SetActive(false);
            _pauseButton.onClick.AddListener(PauseGame);
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;
            _pauseCanvas.SetActive(true);
            GameStateManager.Instance.SetState(GameState.Paused);
        }

    }
}

