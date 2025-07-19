using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.GameRoot;
using IngameDebugConsole;
using UnityEngine;

namespace _Project.Scripts.Commands
{
    public class DebugCommands
    {
        public void Init()
        {
            DebugLogConsole.AddCommand("state", "Show current states", ShowStates);
            DebugLogConsole.AddCommand<Vector3>("cube", "Create cube ", Bootstrap.Instance.SpawnCube);
            DebugLogConsole.AddCommand<string>("loadScene", "Switch current scene", Bootstrap.Instance.LoadScene);
            DebugLogConsole.AddCommand<string>("create", "Create component", CreateComponent);
    }

        void ShowStates()
        {
            Debug.Log("Current states:" +
                      "\n Game: " + Bootstrap.Instance.sm_Game.CurrentState.GetType().Name +  
                      " | Player: " + Bootstrap.Instance.playerController?.statemachine.CurrentState.GetType().Name
                      );
            
        }

        private void CreateComponent(string type)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            ComponentData componentData = new ComponentData
            {
                type = type,
                position = hit.point
            };
            Debug.Log(CircuitManager.Instance.CreateComponent(componentData));
        }
        
    }
}