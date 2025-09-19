using System.Collections.Generic;
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

        private void Awake()
        {
            exitButton.gameObject.SetActive(false);
        }

        public void UpdateGoalList(List<Goal> goals)
        {
            ClearGoalList();
            
            foreach (var goal in goals)
            {
                var item = Instantiate(goalItemPrefab, root).GetComponent<GoalItemController>();
                
                item.Init(goal);
            }
        }

        private void ClearGoalList()
        {
            foreach (var item in root.GetComponentsInChildren<GoalItemController>())
                Destroy(item.gameObject);
        }
    }
}