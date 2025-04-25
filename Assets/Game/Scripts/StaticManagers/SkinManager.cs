using System;
using UnityEngine;

namespace Game
{
    public class SkinManager : MonoBehaviour
    {
        private const string SELECTED_SKIN_KEY = "SelectedSkin";

        public static SkinManager Instance { get; private set; }

        public event Action<SkinType> OnSkinTypeChanged;

        [SerializeField] private SkinType _currentSkinType;
        [SerializeField] private bool clearOnStart;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                if (clearOnStart)
                {
                    ClearSavedSkinType();
                }
                else
                {
                    LoadSavedSkinType();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadSavedSkinType()
        {
            if (PlayerPrefs.HasKey(SELECTED_SKIN_KEY))
            {
                _currentSkinType = (SkinType)PlayerPrefs.GetInt(SELECTED_SKIN_KEY);
                Debug.Log($"Loaded saved skin type: {_currentSkinType}");
            }
            else
            {
                _currentSkinType = SkinType.Original;
                Debug.Log($"No saved skin type found. Setting default: {_currentSkinType}");
            }

            OnSkinTypeChanged?.Invoke(_currentSkinType);
        }

        public void SetSkinType(SkinType skinType)
        {
            if (_currentSkinType != skinType)
            {
                _currentSkinType = skinType;
                PlayerPrefs.SetInt(SELECTED_SKIN_KEY, (int)skinType);
                PlayerPrefs.Save();

                Debug.Log($"Skin type set and saved: {_currentSkinType}");
                OnSkinTypeChanged?.Invoke(_currentSkinType);
            }
        }

        public SkinType GetCurrentSkinType()
        {
            return _currentSkinType;
        }

        private void ClearSavedSkinType()
        {
            PlayerPrefs.DeleteKey(SELECTED_SKIN_KEY);
            PlayerPrefs.Save();
            Debug.Log("Saved skin data cleared.");
        }
    }


}


