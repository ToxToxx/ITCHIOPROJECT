using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GoalManager : MonoBehaviour
    {
        public static GoalManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void CheckWinCondition()
        {
            bool allGoalsMet = true;

            if (allGoalsMet)
            {
                Debug.Log("Все шарики собраны! Победа!");
                GameStateManager.Instance.SetState(GameState.Win);
            }
        }

        public void SetGoalSettings()
        {

        }
    }
}