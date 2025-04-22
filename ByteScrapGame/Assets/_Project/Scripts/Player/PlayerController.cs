using System;
using System.Collections.Generic;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.StateMacines;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        PlayerStateMachine sm_Player;
        
        public void Init(PlayerStateMachine _smPlayer)
        {
            sm_Player = _smPlayer;
        }

        void MoveTo(Vector3 pos)
        {
            
        }
        

    }
}