using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.States.GameStates;
using _Project.Scripts.LevelAndGoals;
using IngameDebugConsole;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Commands
{
    public class DebugCommands
    {
        public void Init()
        {
            DebugLogConsole.AddCommand("state", "Show current states", ShowStates);
            DebugLogConsole.AddCommand<string>("load", "Load level by id", LoadLevel);
            DebugLogConsole.AddCommand("queue", "Show components queue", ShowComponentsQueue);
            DebugLogConsole.AddCommand("menu", "Open menu", GoToMenu);
            DebugLogConsole.AddCommand("goals", "Shows goals", ShowGoals);
        }

        private void ShowComponentsQueue()
        {
            string report = "Components in update queue: \n";
            foreach (var component in CircuitManager.Instance.GetUpdateQueue())
            {
                report += component.name + "\n";
            }
            Debug.Log(report);
            
        }

        void ShowStates()
        {
            Debug.Log("Current states:" +
                      "\n Game: " + Bootstrap.Instance.sm_Game.CurrentState.GetType().Name +  
                      " | Player: " + Bootstrap.Instance.playerController?.statemachine.CurrentState.GetType().Name
                      );
            
        }

        private void LoadLevel(string id)
        {
            var json = Resources.Load<TextAsset>($"Levels/{id}");

            if (json == null)
            {
                Debug.LogError($"Failed to load level {id}");
                return;
            }
            
            var levelData = JsonConvert.DeserializeObject<LevelData>(json.text);
            Bootstrap.Instance.sm_Game.ChangeState(new LoadingLevelState(levelData));
        }

        private void GoToMenu()
        {
            Bootstrap.Instance.sm_Game.ChangeState(new MenuGState());
        }

        private void ShowGoals()
        {
            var msg = "Goal List:\n";
            
            foreach (var goal in Bootstrap.Instance.goalSystem.GetGoals()) // разобраться с терянием инстанса
            {
                msg += $"{(goal.isCompleted ? "V" : "X")}| {goal.name} ({goal.type})\n";
            }
            Debug.Log(msg);
        }
    }
}