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
        
        private GameInput _input;

        public void Init()
        {
            _input = Bootstrap.Instance.input;
            
            movingController = GetComponent<PlayerMovingController>();
            movingController.Init(_input);
            
            
            
            statemachine = GetComponent<PlayerStateMachine>();
            
            interactionController = GetComponent<PlayerInteractionController>();
            interactionController.Init(_input, statemachine);
            
            statemachine.ChangeState(new BuildingPState());
            
        }
        
    }
}