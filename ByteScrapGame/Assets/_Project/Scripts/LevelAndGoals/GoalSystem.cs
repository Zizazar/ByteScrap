using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.ElectricitySystem;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.LevelAndGoals
{
    [Serializable]
    public class Goal
    {
        public string type; // ComponentActivate, ComponentPlace
        public bool isCompleted;
        public string name;
        public Dictionary<string, string> data; // componentType, progress 
    }
    
    
    public class GoalSystem
    {
        
        private List<Goal> _goals = new();

        
        public List<Goal> GetGoals() => _goals;
        
        public void LoadGoals(List<Goal> goals) => _goals = new List<Goal>(goals);

        public void CompleteGoal(Goal goal)
        {
            if (goal == null) return;
            goal.isCompleted = true;
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
        
        
    }
}