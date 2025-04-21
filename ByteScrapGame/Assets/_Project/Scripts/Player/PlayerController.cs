using System;
using System.Collections.Generic;
using _Project.Scripts.GameRoot;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IPlayer, IInitializable
    {
        private Rigidbody _rigidbody;
        private Bootstrap _main;
    
        private IPlayerBehavior _currentBehavior;
        private Dictionary<Type, IPlayerBehavior> _behaviors;

        public void Init(Bootstrap main)
        {
            _main = main;
            _rigidbody = main.GetComponent<Rigidbody>();
        
            _behaviors = new Dictionary<Type, IPlayerBehavior>
            {
                { typeof(MovementBehavior), new MovementBehavior(
                    this, 
                    _main.input, _main.settings) }
            };   
            
            SetBehavior<MovementBehavior>();
        }
        
        public void SetBehavior<T>() where T : IPlayerBehavior
        {
            _currentBehavior?.Exit();
            _currentBehavior = _behaviors[typeof(T)];
            _currentBehavior.Enter();
        }

        public void Look(Vector3 direction)
        {
        
        }

        public void Move(Vector3 direction)
        {
            
        }
        

    }
}