using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class BuildingGState : IState
    {
        public void Enter()
        {
            var playerController = Object.FindAnyObjectByType<PlayerController>();
            playerController.Init();
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    
        public void FixedUpdate()
        {
        }
    }
}