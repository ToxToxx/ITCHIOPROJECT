using UnityEngine;
using UniRx;

namespace Game
{
    public class WallCollider : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _crashEffectPrefab; // <-- сюда закинешь префаб партикла в инспекторе

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerAnimation>(out var playerAnimation))
            {
                playerAnimation.PlayCrashAnimation();
                GameEventSystem.OnPlayerCrashed.OnNext(Unit.Default);

                PlayCrashEffect(collision.transform.position);
            }
        }

        private void PlayCrashEffect(Vector3 position)
        {
            if (_crashEffectPrefab != null)
            {
                // Инстанцируем эффект в позиции удара
                var effect = Instantiate(_crashEffectPrefab, position, Quaternion.identity);

                // Автоудаление после окончания партикла
                Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
            }
        }
    }
}
