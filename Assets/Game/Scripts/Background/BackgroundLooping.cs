using UnityEngine;

namespace Game
{
    public class LoopGround : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;   
        [SerializeField] private float _resetPositionX = -10f; 
        [SerializeField] private float _startPositionX = 10f;  

        private void Update()
        {
            transform.Translate(Vector2.left * _speed * Time.deltaTime);

            if (transform.position.x <= _resetPositionX)
            {
                Vector2 newPos = new Vector2(_startPositionX, transform.position.y);
                transform.position = newPos;
            }
        }
    }
}
