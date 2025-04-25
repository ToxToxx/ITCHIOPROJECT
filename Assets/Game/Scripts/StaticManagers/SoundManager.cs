using UnityEngine;

namespace Game
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioClip _backgroundMusic;

        public bool IsMusicOn = true;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            PlayMusic();
        }

        public void PlayMusic()
        {
            if (_backgroundMusic != null)
            {
                _musicSource.clip = _backgroundMusic;
                _musicSource.loop = true;
                _musicSource.Play();
            }
        }

        public void ToggleMusic()
        {
            IsMusicOn = !IsMusicOn;
            _musicSource.mute = !IsMusicOn;
        }
    }


}

