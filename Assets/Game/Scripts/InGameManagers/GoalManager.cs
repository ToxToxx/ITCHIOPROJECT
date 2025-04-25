using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GoalManager : MonoBehaviour
    {
        public static GoalManager Instance { get; private set; }

        private GameObject _goalManagerUIPrefab;
        private Canvas _targetCanvas;
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

        private void Start()
        {
            if (GoalManagerUI.Instance == null)
            {
                if (_targetCanvas == null)
                {
                    Debug.LogError("❌ Target Canvas не назначен в GoalManager!");
                    return;
                }

                GameObject uiObject = Instantiate(_goalManagerUIPrefab, _targetCanvas.transform, false);
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

        public void SetGoalSettings(GameObject goalManagerUIPrefab, Canvas canvas)
        {
            _goalManagerUIPrefab = goalManagerUIPrefab;
            _targetCanvas = canvas;
        }
    }
}