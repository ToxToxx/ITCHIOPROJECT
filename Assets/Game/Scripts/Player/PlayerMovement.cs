using UniRx;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Настройки движения")]
        [SerializeField] private float moveSpeed = 5f;        // Насколько быстро реагирует на свайп
        [SerializeField] private float maxVerticalSpeed = 10f; // Ограничение максимальной скорости вверх/вниз
        [SerializeField] private float smoothing = 5f;         // Насколько плавно тянется за пальцем

        private float _targetVerticalSpeed;
        private float _currentVerticalSpeed;

        private void Start()
        {
            InputHandler.Instance.OnDragDelta
                .Subscribe(OnDrag)
                .AddTo(this);
        }

        private void Update()
        {
            // Плавно интерполируем к целевой скорости
            _currentVerticalSpeed = Mathf.Lerp(_currentVerticalSpeed, _targetVerticalSpeed, Time.deltaTime * smoothing);

            // Двигаем игрока вверх/вниз
            transform.Translate(Vector3.up * _currentVerticalSpeed * Time.deltaTime);
        }

        private void OnDrag(Vector2 delta)
        {
            // Берем только вертикальное движение
            float verticalInput = delta.y;

            // Преобразуем input в целевую скорость
            _targetVerticalSpeed = Mathf.Clamp(verticalInput * moveSpeed, -maxVerticalSpeed, maxVerticalSpeed);
        }
    }
}
