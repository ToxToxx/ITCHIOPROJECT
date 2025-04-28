using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.UI;

namespace Game
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _entryTextObjects;
        [SerializeField] private TMP_InputField _usernameInputField;

        // Ссылка на кнопку для загрузки лучшего результата
        [SerializeField] private Button _uploadButton;

        private void Start()
        {
            LoadEntries();

            // Добавляем слушатель нажатия кнопки
            _uploadButton.onClick.AddListener(UploadEntry);
        }

        private void LoadEntries()
        {
            // Загружаем данные из лидерборда
            Leaderboards.CluckLeader.GetEntries(entries =>
            {
                // Очищаем предыдущие записи
                foreach (var t in _entryTextObjects)
                    t.text = "";

                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                {
                    // Если имя пустое, заменяем его на "Chicken not registered"
                    string username = string.IsNullOrEmpty(entries[i].Username) ? "Chicken not registered" : entries[i].Username;

                    // Заполняем поле с результатами
                    _entryTextObjects[i].text = $"{entries[i].Rank}. {username} - {entries[i].Score}";
                }

                // Если данных меньше, чем элементов для отображения, заполняем оставшиеся элементы заглушками
                for (int i = entries.Length; i < _entryTextObjects.Length; i++)
                {
                    _entryTextObjects[i].text = "Chicken not registered";
                }
            });
        }

        // Метод для загрузки нового результата в лидерборд (только лучший результат)
        public void UploadEntry()
        {
            int bestScore = GoalManager.Instance.BestScore; // Получаем лучший результат из GoalManager

            // Загружаем только лучший результат
            Leaderboards.CluckLeader.UploadNewEntry(_usernameInputField.text, bestScore, isSuccessful =>
            {
                if (isSuccessful)
                {
                    LoadEntries(); // Перезагружаем список
                }
            });
        }
    }
}
