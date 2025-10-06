using _Project.Scripts.LevelAndGoals;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class GoalItemController : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Color completeColor = Color.green;
        
        public void Init(Goal goal)
        {
            nameText.text = goal.name;
            if (goal.isCompleted)
            {
                nameText.color = completeColor;
            }
            else
            {
                nameText.color = Color.white;
            }
            
        }    
    }
}