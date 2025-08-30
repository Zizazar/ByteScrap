using _Project.Scripts.GameRoot.LevelContexts;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class BuildingGState : IState
    {
        public void Enter()
        {
            Bootstrap.Instance.playerController = Object.FindAnyObjectByType<PlayerController>();
            Bootstrap.Instance.playerController.Init();
            Bootstrap.Instance.ui.componentSelect.Initialize();
        }

        public void Exit()
        {
            BuildingLevelContext buildingLevelContext = Object.FindAnyObjectByType<BuildingLevelContext>();
            buildingLevelContext.DisposeLevel();
        }

        public void Update()
        {
        }
    
        public void FixedUpdate()
        {
        }
    }
}