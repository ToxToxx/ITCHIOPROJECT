using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CanvasSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _currentCanvas;
        [SerializeField] private GameObject _targetCanvas;
        [SerializeField] private Button _switchButton;
        private void Start()
        {
            _switchButton.onClick.AddListener(() =>
            {
                SwitchCanvas();
            });
        }

        public void SwitchCanvas()
        {
            _currentCanvas.SetActive(false);
            _targetCanvas.SetActive(true);
        }

    }
}

