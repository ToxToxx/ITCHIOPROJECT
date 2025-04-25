using System;
using UnityEngine;

namespace Game
{
    public class SpriteSkinController : MonoBehaviour
    {
        [SerializeField] private MapTypeToSprite[] _mapSprites;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Serializable]
        public struct MapTypeToSprite
        {
            public MapType MapType;
            public Sprite Sprite;
        }

        private void OnEnable()
        {
            if (MapManager.Instance != null)
            {
                MapManager.Instance.OnMapTypeChanged += OnMapTypeChanged;
                ApplySprite(MapManager.Instance.GetCurrentMapType());
            }
            else
            {
                ApplyDefaultSprite();
            }
        }

        private void OnDisable()
        {
            if (MapManager.Instance != null)
            {
                MapManager.Instance.OnMapTypeChanged -= OnMapTypeChanged;
            }
        }

        private void OnMapTypeChanged(MapType newMapType)
        {
            ApplySprite(newMapType);
        }

        private void ApplySprite(MapType mapType)
        {
            foreach (var mapping in _mapSprites)
            {
                if (mapping.MapType == mapType)
                {
                    _spriteRenderer.sprite = mapping.Sprite;
                    return;
                }
            }

            Debug.LogWarning($"Sprite for map type {mapType} not found. Using default sprite.");
            ApplyDefaultSprite();
        }

        private void ApplyDefaultSprite()
        {
            if (_mapSprites.Length > 0)
            {
                _spriteRenderer.sprite = _mapSprites[0].Sprite;
                Debug.LogWarning("MapManager not found. Applying the first available sprite.");
            }
            else
            {
                Debug.LogError("No map sprites defined in SpriteSkinController.");
            }
        }
    }
}
