using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.UI;
using Dan.Models; // Убедитесь, что добавили это пространство имен для использования Entry

namespace Game
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _entryTextObjects;
        [SerializeField] private TMP_InputField _usernameInputField;

        // Ссылка на кнопку для загрузки лучшего результата
        [SerializeField] private Button _uploadButton;

        // Ссылка на TMP_Text для отображения личного рекорда
        [SerializeField] private TMP_Text _personalBestText;

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
                // Используем Dan.Models.Entry вместо LeaderboardEntry
                Entry[] leaderboardEntries = entries; // Это массив с типом Entry из Dan.Models

                // Очищаем предыдущие записи
                foreach (var t in _entryTextObjects)
                    t.text = "";

                var length = Mathf.Min(_entryTextObjects.Length, leaderboardEntries.Length);
                for (int i = 0; i < length; i++)
                {
                    // Если имя пустое, заменяем его на "Chicken not registered"
                    string username = string.IsNullOrEmpty(leaderboardEntries[i].Username) ? "Chicken not registered" : leaderboardEntries[i].Username;

                    // Заполняем поле с результатами
                    _entryTextObjects[i].text = $"{leaderboardEntries[i].Rank}. {username} - {leaderboardEntries[i].Score}";
                }

                // Если данных меньше, чем элементов для отображения, заполняем оставшиеся элементы заглушками
                for (int i = leaderboardEntries.Length; i < _entryTextObjects.Length; i++)
                {
                    _entryTextObjects[i].text = "Chicken not registered";
                }

                // Обновляем личный рекорд, если пользователь найден в базе
                UpdatePersonalBest(leaderboardEntries);
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

        private void UpdatePersonalBest(Entry[] leaderboardEntries)
        {
            // Получаем личный рекорд игрока, если он есть в базе данных
            string username = _usernameInputField.text;
            Entry personalBestEntry = System.Array.Find(leaderboardEntries, entry => entry.Username == username);

            if (personalBestEntry.Username != null)
            {
                // Отображаем личный рекорд, если он есть
                _personalBestText.text = $"{personalBestEntry.Rank}. {username} - {personalBestEntry.Score}";
            }
            else
            {
                // Если игрока нет в базе, показываем сообщение
                _personalBestText.text = "No username entered.";
            }
        }
    }
}
