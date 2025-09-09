using System;
using System.Collections.Generic;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.States.PlayerStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class InGameOverlayController : ScreenBase
    {
        
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Image iconImage;
        
        [SerializeField] private List<string> messages = new();
        [SerializeField] private List<Sprite> icons = new();
        [SerializeField] private List<Color> colors = new();

        private void Update()
        {
            switch (Bootstrap.Instance.playerController?.statemachine.CurrentState)
            {
                case BuildingPState: 
                    SetMessage(0);
                    
                    break;
                case InteractPState:
                    SetMessage(1);
                    break;
            }
        }

        private void SetMessage(int i)
        {
            statusText.text = messages[i];
            iconImage.sprite = icons[i];
            
            iconImage.color = colors[i];
            statusText.color = colors[i];
        }
    }
}