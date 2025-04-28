using UnityEngine;
using UniRx;

namespace Game
{
    public class WallSpawner : MonoBehaviour
    {
        [SerializeField] private float _maxTime = 1.5f;
        [SerializeField] private float _heightRange = 0.45f;
        [SerializeField] private GameObject _pipe;
        [SerializeField] private float _destroyingTime = 10f;

        private float _timer;
        private bool _isCrashed = false;

        private void Start()
        {
            SpawnPipe();

            GameEventSystem.OnPlayerCrashed.Subscribe(_ =>
            {
                _isCrashed = true;
            }).AddTo(this);
        }

        private void Update()
        {
            if (_isCrashed || GameStateManager.Instance.CurrentState.Value != GameState.Playing)
                return;

            if (_timer > _maxTime)
            {
                SpawnPipe();
                _timer = 0;
            }

            _timer += Time.deltaTime;
        }

        private void SpawnPipe()
        {
            Vector3 spawnPos = transform.position + new Vector3(0, Random.Range(-_heightRange, _heightRange));
            GameObject pipe = Instantiate(_pipe, spawnPos, Quaternion.identity);
            Destroy(pipe, _destroyingTime);
        }
    }
}
