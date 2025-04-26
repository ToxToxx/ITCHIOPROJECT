using UniRx;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Настройки движения")]
        [SerializeField] private float followSpeed = 5f;    // Скорость следования к позиции пальца
        [SerializeField] private float verticalLimit = 4f;  // Ограничение по высоте движения

        private Camera _mainCamera;
        private float? _targetY;  // Целевая позиция Y (куда хотим двигаться)

        private void Start()
        {
            _mainCamera = Camera.main;

            InputHandler.Instance.OnDragDelta
                .Subscribe(_ => UpdateTargetPosition())
                .AddTo(this);
        }

        private void Update()
        {
            if (_targetY.HasValue)
            {
                // Плавно двигаемся к цели
                Vector3 currentPosition = transform.position;
                float newY = Mathf.Lerp(currentPosition.y, _targetY.Value, Time.deltaTime * followSpeed);

                // Ограничиваем по высоте
                newY = Mathf.Clamp(newY, -verticalLimit, verticalLimit);

                transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
            }
        }

        private void UpdateTargetPosition()
        {
            Vector2 inputPosition = InputHandler.Instance.GetInputPosition();
            if (inputPosition != Vector2.zero)
            {
                Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, -_mainCamera.transform.position.z));
                _targetY = worldPosition.y;
            }
        }
    }
}
