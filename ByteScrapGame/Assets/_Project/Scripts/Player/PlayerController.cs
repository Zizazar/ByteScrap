using System;
using System.Collections.Generic;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.StateMacines;
using _Project.Scripts.GameRoot.States.PlayerStates;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        public PlayerStateMachine statemachine {get; private set;}
        
        public PlayerMovingController movingController;
        public PlayerInteractionController interactionController;

        public void Init()
        {
            
            movingController = GetComponent<PlayerMovingController>();
            movingController.Init();
            
            
            
            statemachine = GetComponent<PlayerStateMachine>();
            
            interactionController = GetComponent<PlayerInteractionController>();
            interactionController.Init();
            
            statemachine.ChangeState(new BuildingPState());
            Bootstrap.Instance.input.Player.Enable();
            Bootstrap.Instance.input.Player.BuildMode.performed += BuildSwitchAction;
            Bootstrap.Instance.input.Player.ExitMenu.performed += ExitMenuAction;
        }

        private void OnDestroy()
        {
            Bootstrap.Instance.input.Player.BuildMode.performed -= BuildSwitchAction;
            Bootstrap.Instance.input.Player.ExitMenu.performed -= ExitMenuAction;
        }

        private void ExitMenuAction(InputAction.CallbackContext obj)
        {
            Bootstrap.Instance.ui.exitMenu.Toggle();
        }


        private void BuildSwitchAction(InputAction.CallbackContext ctx)
        {
            if (statemachine.CurrentState is BuildingPState)
                statemachine.ChangeState(new InteractPState());
            else
                statemachine.ChangeState(new BuildingPState());
        } 
    }
}