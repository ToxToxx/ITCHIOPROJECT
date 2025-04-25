using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoinManager : MonoBehaviour
    {
        private const string COIN_KEY = "PlayerCoins";
        private const string COIN_AWARDED_KEY = "CoinAwarded_{0}";
        private const string COIN_AWARDED_SCENES_KEY = "AwardedScenesList";
        private static CoinManager _instance;
        public static CoinManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CoinManager>();
                    if (_instance == null)
                    {
                        GameObject coinManagerObject = new("CoinManager");
                        _instance = coinManagerObject.AddComponent<CoinManager>();
                    }
                }
                return _instance;
            }
        }

        private int _coinCount;

        public int CoinCount
        {
            get => _coinCount;
            set
            {
                _coinCount = value;
                PlayerPrefs.SetInt(COIN_KEY, _coinCount);
                PlayerPrefs.Save();
            }
        }

        [SerializeField] private bool clearDataOnStart = false;
        [SerializeField] private int testCoinsAmount = 0;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                if (clearDataOnStart)
                {
                    ClearSavedData();
                }
                else
                {
                    LoadCoins();
                }

                if (testCoinsAmount > 0)
                {
                    AddCoins(testCoinsAmount);
                    Debug.Log($"Added {testCoinsAmount} coins for testing purposes.");
                }
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void ClearSavedData()
        {
            PlayerPrefs.DeleteKey(COIN_KEY);
            string scenesList = PlayerPrefs.GetString(COIN_AWARDED_SCENES_KEY, "");
            if (!string.IsNullOrEmpty(scenesList))
            {
                string[] scenes = scenesList.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string scene in scenes)
                {
                    string key = string.Format(COIN_AWARDED_KEY, scene);
                    PlayerPrefs.DeleteKey(key);
                }
                PlayerPrefs.DeleteKey(COIN_AWARDED_SCENES_KEY);
            }

            PlayerPrefs.Save();
            _coinCount = 0;
            Debug.Log("All coin data and awarded scenes have been cleared.");
        }

        private void LoadCoins()
        {
            if (PlayerPrefs.HasKey(COIN_KEY))
            {
                _coinCount = PlayerPrefs.GetInt(COIN_KEY);
            }
            else
            {
                _coinCount = 0;
                PlayerPrefs.SetInt(COIN_KEY, _coinCount);
                PlayerPrefs.Save();
            }
        }

        public void AddCoins(int amount)
        {
            CoinCount += amount;
        }

        public void RemoveCoins(int amount)
        {
            CoinCount -= Mathf.Min(amount, _coinCount);
        }

        public bool HasEnoughCoins(int amount)
        {
            return _coinCount >= amount;
        }

        public bool HasAwardedCoinsForScene(string sceneName)
        {
            return PlayerPrefs.HasKey(string.Format(COIN_AWARDED_KEY, sceneName));
        }

        public void SetCoinsAwardedForScene(string sceneName)
        {
            string key = string.Format(COIN_AWARDED_KEY, sceneName);
            PlayerPrefs.SetInt(key, 1);

            string scenesList = PlayerPrefs.GetString(COIN_AWARDED_SCENES_KEY, "");
            List<string> scenes = new(scenesList.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
            if (!scenes.Contains(sceneName))
            {
                scenes.Add(sceneName);
                PlayerPrefs.SetString(COIN_AWARDED_SCENES_KEY, string.Join(",", scenes));
            }

            PlayerPrefs.Save();
        }
    }

}


