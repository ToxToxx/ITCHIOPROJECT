using System;
using UnityEngine;

namespace Game
{
    public class ModelSkinController : MonoBehaviour
    {
        [SerializeField] private SkinTypeToMaterial[] skinMaterials;
        [SerializeField] private MeshRenderer _meshRenderer;

        [Serializable]
        public struct SkinTypeToMaterial
        {
            public SkinType SkinType;
            public Material[] Materials;
        }

        private void OnEnable()
        {
            if (SkinManager.Instance != null)
            {
                SkinManager.Instance.OnSkinTypeChanged += OnSkinTypeChanged;
                ApplySkin(SkinManager.Instance.GetCurrentSkinType());
            }
            else
            {
                ApplyDefaultSkin();
            }
        }

        private void OnDisable()
        {
            if (SkinManager.Instance != null)
                SkinManager.Instance.OnSkinTypeChanged -= OnSkinTypeChanged;
        }

        private void OnSkinTypeChanged(SkinType newSkinType)
        {
            ApplySkin(newSkinType);
        }

        private void ApplySkin(SkinType skinType)
        {
            foreach (var skinMaterial in skinMaterials)
            {
                if (skinMaterial.SkinType == skinType)
                {
                    _meshRenderer.materials = skinMaterial.Materials;
                    return;
                }
            }

            Debug.LogWarning($"Materials for skin type {skinType} not found. Using default materials.");
            ApplyDefaultSkin();
        }

        private void ApplyDefaultSkin()
        {
            if (skinMaterials.Length > 0)
            {
                _meshRenderer.materials = skinMaterials[0].Materials;
                Debug.LogWarning("SkinManager not found. Applying the first available skin.");
            }
            else
            {
                Debug.LogError("No skin materials defined in ModelSkinController.");
            }
        }
    }
}
