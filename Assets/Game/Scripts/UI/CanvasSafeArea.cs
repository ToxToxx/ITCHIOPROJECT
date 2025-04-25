using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class CanvasSafeArea : MonoBehaviour
    {
        [Header("Safe Area Settings")]
        [SerializeField] private bool _shouldApplyToChildren = true; // Determines if safe area adjustments should be applied to child elements
        [SerializeField] private Vector2 _additionalSafeAreaPadding = Vector2.zero; // Additional padding from the edges of the safe area
        [SerializeField] private List<RectTransform> _excludedChildren = new List<RectTransform>(); // List of child elements that should not be adjusted

        private RectTransform _canvasRectTransform;
        private Rect _lastAppliedSafeArea;
        private Vector2Int _lastScreenResolution;
        private int _lastChildCount;

        private void Awake()
        {
            _canvasRectTransform = GetComponent<RectTransform>();
            _lastChildCount = transform.childCount;
            AdjustCanvasToSafeArea();
        }

        private void Update()
        {
            // Check for changes in safe area, screen resolution, or child count
            bool hasSafeAreaChanged = _lastAppliedSafeArea != Screen.safeArea;
            bool hasScreenResolutionChanged = _lastScreenResolution != new Vector2Int(Screen.width, Screen.height);
            bool hasChildCountChanged = _lastChildCount != transform.childCount;

            if (hasSafeAreaChanged || hasScreenResolutionChanged || hasChildCountChanged)
            {
                AdjustCanvasToSafeArea();
            }
        }

        private void AdjustCanvasToSafeArea()
        {
            // Update the last known values
            _lastChildCount = transform.childCount;
            _lastAppliedSafeArea = Screen.safeArea;
            _lastScreenResolution = new Vector2Int(Screen.width, Screen.height);

            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 safeAreaAnchorMin = CalculateSafeAreaAnchorMin(screenSize);
            Vector2 safeAreaAnchorMax = CalculateSafeAreaAnchorMax(screenSize);

            // Apply the safe area to the canvas
            ApplySafeAreaToCanvas(safeAreaAnchorMin, safeAreaAnchorMax);

            // Adjust child elements if enabled
            if (_shouldApplyToChildren)
            {
                AdjustChildElements(screenSize);
            }

            Debug.Log($"Safe Area applied: {_lastAppliedSafeArea}, Anchors: Min={safeAreaAnchorMin}, Max={safeAreaAnchorMax}, Child Count: {_lastChildCount}");
        }

        private Vector2 CalculateSafeAreaAnchorMin(Vector2 screenSize)
        {
            Vector2 anchorMin = _lastAppliedSafeArea.position / screenSize;
            anchorMin.x += _additionalSafeAreaPadding.x / screenSize.x;
            anchorMin.y += _additionalSafeAreaPadding.y / screenSize.y;
            return anchorMin;
        }

        private Vector2 CalculateSafeAreaAnchorMax(Vector2 screenSize)
        {
            Vector2 anchorMax = (_lastAppliedSafeArea.position + _lastAppliedSafeArea.size) / screenSize;
            anchorMax.x -= _additionalSafeAreaPadding.x / screenSize.x;
            anchorMax.y -= _additionalSafeAreaPadding.y / screenSize.y;
            return anchorMax;
        }

        private void ApplySafeAreaToCanvas(Vector2 anchorMin, Vector2 anchorMax)
        {
            _canvasRectTransform.anchorMin = anchorMin;
            _canvasRectTransform.anchorMax = anchorMax;
            _canvasRectTransform.offsetMin = Vector2.zero;
            _canvasRectTransform.offsetMax = Vector2.zero;
        }

        private void AdjustChildElements(Vector2 screenSize)
        {
            RectTransform[] childElements = GetComponentsInChildren<RectTransform>();
            foreach (RectTransform child in childElements)
            {
                // Skip the canvas itself and any excluded children
                if (child == _canvasRectTransform || _excludedChildren.Contains(child)) continue;

                // Store the current properties of the child element
                Vector2 childAnchorMin = child.anchorMin;
                Vector2 childAnchorMax = child.anchorMax;
                Vector2 childOffsetMin = child.offsetMin;
                Vector2 childOffsetMax = child.offsetMax;

                // Calculate the child's bounds in normalized coordinates
                Vector2 childMin = childAnchorMin;
                Vector2 childMax = childAnchorMax;

                // Convert to screen coordinates
                Vector2 childMinScreen = childMin * screenSize;
                Vector2 childMaxScreen = childMax * screenSize;

                // Get the safe area bounds in screen coordinates
                Vector2 safeAreaMin = _lastAppliedSafeArea.position;
                Vector2 safeAreaMax = _lastAppliedSafeArea.position + _lastAppliedSafeArea.size;

                // Adjust the child's position if it falls outside the safe area
                if (childMinScreen.y < safeAreaMin.y)
                {
                    float delta = (safeAreaMin.y - childMinScreen.y) / screenSize.y;
                    childAnchorMin.y += delta;
                    childAnchorMax.y += delta;
                }
                if (childMaxScreen.y > safeAreaMax.y)
                {
                    float delta = (childMaxScreen.y - safeAreaMax.y) / screenSize.y;
                    childAnchorMin.y -= delta;
                    childAnchorMax.y -= delta;
                }
                if (childMinScreen.x < safeAreaMin.x)
                {
                    float delta = (safeAreaMin.x - childMinScreen.x) / screenSize.x;
                    childAnchorMin.x += delta;
                    childAnchorMax.x += delta;
                }
                if (childMaxScreen.x > safeAreaMax.x)
                {
                    float delta = (childMaxScreen.x - safeAreaMax.x) / screenSize.x;
                    childAnchorMin.x -= delta;
                    childAnchorMax.x -= delta;
                }

                // Apply the adjusted anchors and offsets
                child.anchorMin = childAnchorMin;
                child.anchorMax = childAnchorMax;
                child.offsetMin = childOffsetMin;
                child.offsetMax = childOffsetMax;
            }
        }
    }
}