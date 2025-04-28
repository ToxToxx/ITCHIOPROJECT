using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ManagersBootstrap : MonoBehaviour
    {
        [Header("GameOver Settings")]
        [SerializeField] private GameObject _gameOverCanvas;

        [Header("Goal Settings")]
        [SerializeField] private float _pointsPerSecond = 1f;

        private void Awake()
        {
            if (_gameOverCanvas == null)
            {
                Debug.LogError("One or more required fields are not set in ManagersBootstrap!");
                return;
            }

            var inGameManagers = gameObject.AddComponent<InGameManagersBootstrap>();
            var goalManager = gameObject.AddComponent<GoalManager>();
            var inputHandler = gameObject.AddComponent<InputHandler>();

            inGameManagers.SetGameOverSettings(_gameOverCanvas);
            goalManager.SetGoalSettings(_pointsPerSecond);

            inGameManagers.InitializeManagers();
        }
    }
}
