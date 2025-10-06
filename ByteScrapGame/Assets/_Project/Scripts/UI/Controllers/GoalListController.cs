using System;
using System.Collections.Generic;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.States.GameStates;
using _Project.Scripts.LevelAndGoals;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class GoalListController : MonoBehaviour
    {
        [SerializeField] private GameObject goalItemPrefab;
        [SerializeField] private Transform root;
        
        [SerializeField] private Button exitButton;

        private List<GameObject> _goalItems = new();
        
        public void Init()
        {
            exitButton.gameObject.SetActive(false);
            Bootstrap.Instance.goalSystem.OnCompleted.AddListener(OnGoalCompleted);
            exitButton.onClick.AddListener(OnExitButtonClicked);
            OnGoalCompleted();
        }

        private void OnDestroy()
        {
            Bootstrap.Instance.goalSystem.OnCompleted.RemoveListener(OnGoalCompleted);
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnGoalCompleted()
        {
            var goals = Bootstrap.Instance.goalSystem.GetGoals();
            UpdateGoalList(goals);

            if (Bootstrap.Instance.goalSystem.IsAllGoalsCompleted())
            {
                exitButton.gameObject.SetActive(true);
            }   
        }

        private void OnExitButtonClicked()
        {
            Bootstrap.Instance.sm_Game.ChangeState(new MenuGState());
            exitButton.gameObject.SetActive(false);
        }

        public void UpdateGoalList(List<Goal> goals)
        {
            ClearGoalList();
            
            foreach (var goal in goals)
            {
                var item = Instantiate(goalItemPrefab, root);
                _goalItems.Add(item);
                item.GetComponent<GoalItemController>().Init(goal);
            }
        }

        private void ClearGoalList()
        {
            foreach (var item in _goalItems)
                Destroy(item);
        }
    }
}