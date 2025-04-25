using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private Sprite soundOnSprite;
        [SerializeField] private Sprite soundOffSprite;

        [SerializeField] private Image buttonImage;
        [SerializeField] private Button _soundButton;

        private void Start()
        {
            UpdateButtonSprite();

            _soundButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.ToggleMusic();
                UpdateButtonSprite();
            });
        }

        private void UpdateButtonSprite()
        {
            buttonImage.sprite = SoundManager.Instance.IsMusicOn ? soundOnSprite : soundOffSprite;
        }


    }

}
