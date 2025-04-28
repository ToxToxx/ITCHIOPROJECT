using UnityEngine;
using UniRx;

namespace Game
{
    public class WallMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.65f;
        private bool _isCrashed = false;

        private void Start()
        {
            GameEventSystem.OnPlayerCrashed.Subscribe(_ =>
            {
                _isCrashed = true;
            }).AddTo(this);
        }

        private void Update()
        {
            if (GameStateManager.Instance.CurrentState.Value == GameState.Playing && !_isCrashed)
            {
                transform.position += Vector3.left * _speed * Time.deltaTime;
            }
        }
    }
}
