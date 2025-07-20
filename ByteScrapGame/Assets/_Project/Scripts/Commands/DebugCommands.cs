using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.States.GameStates;
using IngameDebugConsole;
using UnityEngine;

namespace _Project.Scripts.Commands
{
    public class DebugCommands
    {
        public void Init()
        {
            DebugLogConsole.AddCommand("state", "Show current states", ShowStates);
            DebugLogConsole.AddCommand<string>("loadScene", "Switch current scene", loadScene);
            DebugLogConsole.AddCommand("show_queue", "Show components queue", ShowComponentsQueue);
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

        private void loadScene(string sceneName)
        {
            Bootstrap.Instance.LevelToLoad = sceneName;
            Bootstrap.Instance.sm_Game.ChangeState(new LoadingLevelState());
        }
        
    }
}