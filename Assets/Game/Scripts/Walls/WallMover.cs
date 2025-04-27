using UnityEngine;

namespace Game
{
    public class WallMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.65f;

        private void Update()
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;
        }
    }
}