using System;
using _Project.Scripts.GameRoot.StateMacines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
    public class PlayerInteractionController : MonoBehaviour
    {
        private GameInput _input;
        private PlayerStateMachine _stateMachine;


        public void Init(GameInput input, PlayerStateMachine statemachine)
        {
            _input = input;
            _stateMachine = statemachine;
            
            EnableInteraction();
        }

        private void OnDestroy()
        {
            DisableInteraction();
        }

        private void InteractionAction(InputAction.CallbackContext ctx)
        {
            if (Camera.main)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                    
                IInteractable interactable =
                    hit.collider.gameObject.GetComponent(typeof(IInteractable)) as IInteractable;
                interactable?.OnInteract();
                Debug.Log(interactable);
                Debug.Log(hit.collider.gameObject);
            }
            else Debug.LogError("Main Camera isn't set! Interact system is disabled.");
        }

        public void EnableInteraction()
        {
            _input.Player.Interact.performed += InteractionAction;
        }

        public void DisableInteraction()
        {
            _input.Player.Interact.performed -= InteractionAction;
        }
        
    }
}