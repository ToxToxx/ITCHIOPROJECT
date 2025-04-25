using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;

        private void Awake()
        {
            _restartButton.onClick.AddListener(RestartGame);
        }

        private void RestartGame()
        {
            GameStateManager.Instance.SetState(GameState.Playing);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}

