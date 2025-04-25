using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ManagersBootstrap : MonoBehaviour
    {
        [Header("Win Settings")]
        [SerializeField] private GameObject _winCanvas;
        [SerializeField] private int _reward = 0;
        [SerializeField] private int _currentLevelIndex = 1;

        [Header("GameOver Settings")]
        [SerializeField] private GameObject _gameOverCanvas;

        [Header("Input Settings")]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private float _rayDistance = 100f;


        private void Awake()
        {
            // Проверяем, что все поля установлены
            if (_winCanvas == null || _gameOverCanvas == null)
            {
                Debug.LogError("One or more required fields are not set in ManagersBootstrap!");
                return;
            }

            // Добавляем нужные компоненты
            var inGameManagers = gameObject.AddComponent<InGameManagersBootstrap>();
            var goalManager = gameObject.AddComponent<GoalManager>();
            var inputHandler = gameObject.AddComponent<InputHandler>();
             
            // Устанавливаем настройки для менеджеров
            inGameManagers.SetWinSettings(_winCanvas, _reward, _currentLevelIndex);
            inGameManagers.SetGameOverSettings(_gameOverCanvas);
            goalManager.SetGoalSettings();
            inputHandler.SetInputSettings(_targetLayer, _rayDistance);

            // Вызываем дополнительную инициализацию
            inGameManagers.InitializeManagers();
        }
    }
}