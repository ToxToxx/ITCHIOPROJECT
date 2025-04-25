using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class SkinPackButton : MonoBehaviour
    {
        public enum SkinPackType
        {
            Skin,
            Map
        }

        [System.Serializable]
        public class SkinPack
        {
            public string packName;
            public SkinPackType packType;
            public SkinType skinType;
            public MapType mapType;
            public int price;
            public bool isUnlocked;
        }

        [SerializeField] private SkinPack _skinPack;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _skinButton;

        private void Start()
        {
            LoadUnlockedState();
            UpdateUI();

            _skinButton.onClick.AddListener(OnButtonClick);

            DOVirtual.DelayedCall(0.1f, UpdateUI);

            if (_skinPack.packType == SkinPackType.Skin)
            {
                if (SkinManager.Instance != null)
                    SkinManager.Instance.OnSkinTypeChanged += OnSkinChanged;
            }
            else if (_skinPack.packType == SkinPackType.Map)
            {
                if (MapManager.Instance != null)
                    MapManager.Instance.OnMapTypeChanged += OnMapChanged;
            }
        }

        private void OnDestroy()
        {
            if (_skinPack.packType == SkinPackType.Skin)
            {
                if (SkinManager.Instance != null)
                    SkinManager.Instance.OnSkinTypeChanged -= OnSkinChanged;
            }
            else if (_skinPack.packType == SkinPackType.Map)
            {
                if (MapManager.Instance != null)
                    MapManager.Instance.OnMapTypeChanged -= OnMapChanged;
            }
        }

        private void OnSkinChanged(SkinType newSkinType)
        {
            UpdateUI();
        }

        private void OnMapChanged(MapType newMapType)
        {
            UpdateUI();
        }

        private void Update()
        {
            UpdateUI();
        }

        private void LoadUnlockedState()
        {
            if (PlayerPrefs.GetInt(_skinPack.packName, 0) == 1)
            {
                _skinPack.isUnlocked = true;
            }
        }

        public void OnButtonClick()
        {
            if (_skinPack.isUnlocked)
            {
                ApplySkinPack();
            }
            else
            {
                AttemptToPurchaseSkinPack();
            }
        }

        private void AttemptToPurchaseSkinPack()
        {
            if (CoinManager.Instance.HasEnoughCoins(_skinPack.price))
            {
                CoinManager.Instance.RemoveCoins(_skinPack.price);
                _skinPack.isUnlocked = true;
                PlayerPrefs.SetInt(_skinPack.packName, 1);
                PlayerPrefs.Save();
                Debug.Log($"Purchased skin pack: {_skinPack.packName}");
                ApplySkinPack();
            }
            else
            {
                Debug.Log("Not enough coins to purchase skin pack!");
            }
        }

        private void ApplySkinPack()
        {
            if (_skinPack.packType == SkinPackType.Skin)
            {
                Debug.Log($"Applying skin: {_skinPack.skinType}");
                SkinManager.Instance.SetSkinType(_skinPack.skinType);
            }
            else if (_skinPack.packType == SkinPackType.Map)
            {
                Debug.Log($"Applying map: {_skinPack.mapType}");
                MapManager.Instance.SetMapType(_skinPack.mapType);
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_skinPack.isUnlocked)
            {
                bool isCurrent = false;
                if (_skinPack.packType == SkinPackType.Skin)
                {
                    isCurrent = SkinManager.Instance.GetCurrentSkinType() == _skinPack.skinType;
                }
                else if (_skinPack.packType == SkinPackType.Map)
                {
                    isCurrent = MapManager.Instance.GetCurrentMapType() == _skinPack.mapType;
                }

                _priceText.text = isCurrent ? "Curr" : "Select";

            }
            else
            {
                _priceText.text = FormatPrice(_skinPack.price);
            }
        }



        private string FormatPrice(int price)
        {
            return price >= 1000 ? (price / 1000).ToString() + "K" : price.ToString();
        }
    }
}
