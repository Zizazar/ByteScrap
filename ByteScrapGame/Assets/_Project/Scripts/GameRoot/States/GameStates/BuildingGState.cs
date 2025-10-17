using _Project.Scripts.GameRoot.LevelContexts;
using _Project.Scripts.LevelAndGoals;
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
            Bootstrap.Instance.ui.inGameOverlay.Open();
        }

        public void Exit()
        {
            Bootstrap.Instance.ui.inGameOverlay.Close();
        }

        public void Update()
        {
            
        }
    
        public void FixedUpdate()
        {
        }
    }
}