using System;
using System.Linq;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem.Components
{
    public class SwitchComponent : CircuitComponent, IInteractable
    {
        private bool _isOn;

        public override void Init()
        {
            componentType = "Switch";
        }

        public void OnInteract()
        {
            Debug.Log("toggled!");
            
        }

        public override void UpdateLogic()
        {
            Pin input = GetPin(isInput: true);
            Pin output = GetPin(isInput: false);
            if (!input || !output) return;
            
            if (_isOn)
            {
                output.Voltage = input.Voltage;
            }
            else
            {
                output.Voltage = 0;
            }
                
        }
    }
}