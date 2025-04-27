using UniRx;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Настройки движения")]
        [SerializeField] private float ascendSpeed = 5f;      // Скорость подъема при зажатии
        [SerializeField] private float descendSpeed = 3f;     // Скорость падения без зажатия
        [SerializeField] private float maxVerticalSpeed = 7f; // Ограничение максимальной скорости
        [SerializeField] private float verticalLimit = 4f;    // Ограничение по высоте

        private bool _isTouching;
        private float _verticalVelocity;

        private void Start()
        {
            // Реактивно отслеживаем касания
            Observable.EveryUpdate()
                .Subscribe(_ => UpdateTouchState())
                .AddTo(this);
        }

        private void UpdateTouchState()
        {
            _isTouching = Input.touchCount > 0 || Input.GetMouseButton(0);
        }

        private void Update()
        {
            if (_isTouching)
            {
                _verticalVelocity += ascendSpeed * Time.deltaTime;
            }
            else
            {
                _verticalVelocity -= descendSpeed * Time.deltaTime;
            }

            // Ограничиваем скорость
            _verticalVelocity = Mathf.Clamp(_verticalVelocity, -maxVerticalSpeed, maxVerticalSpeed);

            // Обновляем позицию
            Vector3 currentPosition = transform.position;
            float newY = currentPosition.y + _verticalVelocity * Time.deltaTime;

            // Ограничиваем позицию по высоте
            newY = Mathf.Clamp(newY, -verticalLimit, verticalLimit);

            transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
        }
    }
}
