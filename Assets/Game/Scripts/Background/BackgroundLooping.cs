using UnityEngine;

namespace Game
{
    public class LoopGround : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;   // Скорость движения фона
        [SerializeField] private float _resetPositionX = -10f; // Позиция, где фон сбрасывается
        [SerializeField] private float _startPositionX = 10f;  // Позиция, куда фон возвращается после сброса

        private void Update()
        {
            // Двигаем фон влево
            transform.Translate(Vector2.left * _speed * Time.deltaTime);

            // Если фон ушёл за пределы видимости, сбрасываем его позицию
            if (transform.position.x <= _resetPositionX)
            {
                Vector2 newPos = new Vector2(_startPositionX, transform.position.y);
                transform.position = newPos;
            }
        }
    }
}
