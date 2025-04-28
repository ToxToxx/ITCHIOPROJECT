using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ManagersBootstrap : MonoBehaviour
    {
        [Header("GameOver Settings")]
        [SerializeField] private GameObject _gameOverCanvas;


        private void Awake()
        {
            // Проверяем, что все поля установлены
            if (_gameOverCanvas == null)
            {
                Debug.LogError("One or more required fields are not set in ManagersBootstrap!");
                return;
            }

            // Добавляем нужные компоненты
            var inGameManagers = gameObject.AddComponent<InGameManagersBootstrap>();
            var goalManager = gameObject.AddComponent<GoalManager>();
            var inputHandler = gameObject.AddComponent<InputHandler>();
             
            // Устанавливаем настройки для менеджеров
            inGameManagers.SetGameOverSettings(_gameOverCanvas);
            goalManager.SetGoalSettings();

            // Вызываем дополнительную инициализацию
            inGameManagers.InitializeManagers();
        }
    }
}