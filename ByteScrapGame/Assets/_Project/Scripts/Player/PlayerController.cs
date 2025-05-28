using System;
using System.Collections.Generic;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.StateMacines;
using _Project.Scripts.GameRoot.States.PlayerStates;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        public PlayerStateMachine statemachine {get; private set;}
        public Transform[] waypoints;
        
        private GameInput _input;
        private Tweener _moveTween;
        private int _currentIndex;

        public void Init()
        {
            _input = Bootstrap.Instance.input;
            
            statemachine = GetComponent<PlayerStateMachine>();
            statemachine.ChangeState(new BuildingPState());
            transform.position = waypoints[0].position;
        }

        public void HandleInteraction()
        {
            if (Camera.main)
            {
                if (Input.GetMouseButtonDown(0))
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
            }
            else
            {
                Debug.LogError("Main Camera isn't set! Interact system is disabled.");
            }
        }

        private void Update()
        {
            // !!!Временно!!!
            if (Input.GetKeyDown(KeyCode.D))
            {
                _currentIndex = (_currentIndex + 1) % waypoints.Length;
                MoveToWaypoint(_currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                _currentIndex--;
                if (_currentIndex < 0) _currentIndex = waypoints.Length - 1;
                MoveToWaypoint(_currentIndex);
            }
        }
        
        private void MoveToWaypoint(int index)
        {
            // TODO: Иницилизация вайпоинтов при заргузке на уровень из LevelController
            if (_moveTween != null && _moveTween.IsActive())
            {
                _moveTween.Kill();
            }   

            
            _moveTween = transform.DOMove(waypoints[index].position, 1f)
                .SetEase(Ease.InOutFlash)
                .OnComplete(() => Debug.Log("Reached waypoint: " + index));
            
        }
        
    }
}