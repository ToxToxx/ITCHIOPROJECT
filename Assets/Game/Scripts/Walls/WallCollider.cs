using UnityEngine;
using UniRx;

namespace Game
{
    public class WallCollider : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerAnimation>(out var playerAnimation))
            {
                playerAnimation.PlayCrashAnimation();
                GameEventSystem.OnPlayerCrashed.OnNext(Unit.Default);
            }
        }
    }
}
