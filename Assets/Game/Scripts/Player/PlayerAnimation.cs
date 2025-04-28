using UnityEngine;
using DG.Tweening;

namespace Game
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private float _bounceForce = 5f;
        [SerializeField] private float _fallSpeed = -5f;
        [SerializeField] private float _rotationSpeed = 360f;
        [SerializeField] private float _flashDuration = 0.1f;
        [SerializeField] private int _flashCount = 5;
        [SerializeField] private float _cameraShakeDuration = 0.2f;   // <-- длительность тряски камеры
        [SerializeField] private float _cameraShakeStrength = 0.3f;   // <-- сила тряски камеры

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private bool _isCrashed = false;
        private Sequence _flashSequence;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_particleSystem == null)
            {
                _particleSystem = GetComponentInChildren<ParticleSystem>();
            }
        }

        public void PlayCrashAnimation()
        {
            if (_isCrashed) return;
            _isCrashed = true;

            // Останавливаем партиклы
            if (_particleSystem != null)
            {
                _particleSystem.Stop();
            }

            // Динамически добавляем Rigidbody2D
            _rigidbody = gameObject.AddComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0f; // отключаем гравитацию, управляем вручную
            _rigidbody.freezeRotation = false;

            // Применяем отскок назад
            _rigidbody.linearVelocity = new Vector2(-_bounceForce, _bounceForce);

            // Включаем вращение
            _rigidbody.angularVelocity = _rotationSpeed;

            // Запускаем мигание
            PlayFlashRed();

            // Запускаем тряску камеры
            ShakeCamera();

            // Через время вызвать GameOver
            Invoke(nameof(TriggerGameOver), _flashDuration * _flashCount * 2);
        }

        private void PlayFlashRed()
        {
            Color originalColor = _spriteRenderer.color;
            Color flashColor = Color.red;

            _flashSequence = DOTween.Sequence();

            for (int i = 0; i < _flashCount; i++)
            {
                _flashSequence.Append(_spriteRenderer.DOColor(flashColor, _flashDuration));
                _flashSequence.Append(_spriteRenderer.DOColor(originalColor, _flashDuration));
            }

            _flashSequence.SetEase(Ease.Linear);
        }

        private void ShakeCamera()
        {
            if (Camera.main != null)
            {
                Camera.main.transform
                    .DOShakePosition(_cameraShakeDuration, _cameraShakeStrength, vibrato: 10, randomness: 90, snapping: false, fadeOut: true)
                    .SetEase(Ease.OutQuad);
            }
        }

        private void TriggerGameOver()
        {
            GameStateManager.Instance.SetState(GameState.Result);
        }

        private void Update()
        {
            if (_isCrashed && _rigidbody != null)
            {
                // Постоянно тянем вниз с заданной скоростью
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _fallSpeed);
            }
        }

        private void OnDestroy()
        {
            if (_flashSequence != null && _flashSequence.IsActive())
            {
                _flashSequence.Kill();
            }
        }
    }
}
