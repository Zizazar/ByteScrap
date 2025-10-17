using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.States.GameStates;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.LevelAndGoals
{
    [Serializable]
    public class Goal
    {
        public string type; // ComponentActivate, ComponentPlace
        public bool isCompleted;
        public string name;
        public Dictionary<string, string> data; // componentType, progress 

        public override string ToString()
        {
            return $"{type}: {name} \n {data}";
        }
    }
    
    
    public class GoalSystem
    {
        public UnityEvent OnCompleted = new();
        
        private List<Goal> _goals = new();

        
        public List<Goal> GetGoals() => _goals;
        
        public bool IsAllGoalsCompleted() => _goals.All(goal => goal.isCompleted);
        
        public void LoadGoals(List<Goal> goals)
        {
            if (goals != null) _goals = new List<Goal>(goals);
        }

        public void CompleteGoal(Goal goal)
        {
            if (goal == null) return;
            goal.isCompleted = true;
            OnCompleted.Invoke();
            AutoExitCheck();
        }

        private void AutoExitCheck()
        {
            if (IsAllGoalsCompleted() && Bootstrap.Instance.gameSettings.levelAutoExit)
                Bootstrap.Instance.CloseLevel();
        }


        public void TriggerComponentPlace(CircuitComponent component)
        {
            CompleteGoal(_goals.FirstOrDefault(goal =>
                goal.type == "ComponentPlace" && 
                goal.data.GetValueOrDefault("ComponentType") == component.ComponentType &&
                !goal.isCompleted
                ));
        }

        public void TriggerComponentActivate(CircuitComponent component)
        {
            CompleteGoal(_goals.FirstOrDefault(goal =>
                goal.type == "ComponentActivate" && 
                goal.data.GetValueOrDefault("ComponentType") == component.ComponentType &&
                !goal.isCompleted
            ));
        }

        public void TriggerComponentChangeProperty(CircuitComponent component)
        {
            Debug.Log(_goals[1]);
            var props = component.GetProperties();
            CompleteGoal(_goals.FirstOrDefault(goal =>
                goal.type == "ComponentProperties" && 
                goal.data.GetValueOrDefault("ComponentType") == component.ComponentType &&
                props.All(p => 
                    goal.data.TryGetValue(p.Key, out string value) && value == p.Value
                    ) &&
                !goal.isCompleted
            ));
        }
        
        
    }
}