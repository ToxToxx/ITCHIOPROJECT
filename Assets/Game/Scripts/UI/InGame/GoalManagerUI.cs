using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class GoalManagerUI : MonoBehaviour
    {
        public static GoalManagerUI Instance { get; private set; }


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
    }
}