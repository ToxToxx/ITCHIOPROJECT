using System;
using UnityEngine;

namespace Game
{
    public class MapManager : MonoBehaviour
    {
        private const string SELECTED_MAP_KEY = "SelectedMap";

        public static MapManager Instance { get; private set; }

        public event Action<MapType> OnMapTypeChanged;

        [SerializeField] private MapType _currentMapType;
        [SerializeField] private bool clearOnStart = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                if (clearOnStart)
                    ClearSavedMapType();
                else
                    LoadSavedMapType();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadSavedMapType()
        {
            if (PlayerPrefs.HasKey(SELECTED_MAP_KEY))
            {
                _currentMapType = (MapType)PlayerPrefs.GetInt(SELECTED_MAP_KEY);
                Debug.Log($"Loaded saved map type: {_currentMapType}");
            }
            else
            {
                _currentMapType = MapType.Original;
                Debug.Log($"No saved map type found. Setting default: {_currentMapType}");
            }
            OnMapTypeChanged?.Invoke(_currentMapType);
        }

        public void SetMapType(MapType mapType)
        {
            if (_currentMapType != mapType)
            {
                _currentMapType = mapType;
                PlayerPrefs.SetInt(SELECTED_MAP_KEY, (int)mapType);
                PlayerPrefs.Save();
                Debug.Log($"Map type set and saved: {_currentMapType}");
                OnMapTypeChanged?.Invoke(_currentMapType);
            }
        }

        public MapType GetCurrentMapType()
        {
            return _currentMapType;
        }

        private void ClearSavedMapType()
        {
            PlayerPrefs.DeleteKey(SELECTED_MAP_KEY);
            PlayerPrefs.Save();
            Debug.Log("Saved map data cleared.");
        }
    }
}
