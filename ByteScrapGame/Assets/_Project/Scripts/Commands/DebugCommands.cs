using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.GameRoot;
using IngameDebugConsole;
using UnityEngine;

namespace _Project.Scripts.Commands
{
    public class DebugCommands : IInitializable
    {
        private Bootstrap _m;

        public void Init(Bootstrap main)
        {
            _m = main;

            DebugLogConsole.AddCommand("state", "Show current states", ShowStates);
            DebugLogConsole.AddCommand<Vector3>( "cube", "Create cube ", _m.SpawnCube );
        }

        void ShowStates()
        {
            Debug.Log("Current states:" +
                      "\n Game: " + Bootstrap.Instance.sm_Game.CurrentState.GetType().Name +  
                      " | Player: " + Bootstrap.Instance.sm_Player.CurrentState.GetType().Name
                      );
            
        }

        
        
        
    }
}