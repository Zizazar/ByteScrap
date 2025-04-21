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
            
            DebugLogConsole.AddCommand<string, string>( "connect", "Connected", ES_Connect );
            DebugLogConsole.AddCommand<Vector3>( "cube", "Created cube ", _m.SpawnCube );
        }

        void ES_Connect(string pin1, string pin2)
        {
            
        }

        
        
        
    }
}