using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class LoopingImageMover : MonoBehaviour
    {
        [SerializeField] private RectTransform imageRect;
        [SerializeField] private float travelDistance = 1000f; // Насколько далеко летит
        [SerializeField] private float duration = 2f;
        [SerializeField] private float delay = 0.5f;
        [SerializeField] private bool moveRight = true;

        private Vector2 startPosition;
        private Vector2 targetPosition;

        private void Start()
        {
            if (imageRect == null)
                imageRect = GetComponent<RectTransform>();

            startPosition = imageRect.anchoredPosition;

            float direction = moveRight ? 1f : -1f;
            targetPosition = startPosition + Vector2.right * travelDistance * direction;

            StartLoop();
        }

        private void StartLoop()
        {
            imageRect.anchoredPosition = startPosition;

            DOTween.Sequence()
                .Append(imageRect.DOAnchorPos(targetPosition, duration).SetEase(Ease.Linear))
                .AppendInterval(delay)
                .AppendCallback(() => imageRect.anchoredPosition = startPosition)
                .AppendInterval(delay)
                .SetLoops(-1);
        }
    }
}
