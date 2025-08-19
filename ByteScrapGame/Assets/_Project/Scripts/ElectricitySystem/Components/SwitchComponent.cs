using System.Collections.Generic;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem.Components
{

    public class SwitchComponent : CircuitComponent, IInteractable
    {
        public bool isOn = true;
        
        public void OnInteract()
        {
            isOn = !isOn;
            Debug.Log($"Toggle Switch: {isOn}");
            CircuitManager.Instance.RequestCircuitUpdate();
        }

        public override void ReceiveSignal(Direction fromDirection, bool signal)
        {
            if (signal) newState = true;
        }

        public override bool GetOutput(Direction direction)
        {
            return isOn && currentState;
        }

        protected override Dictionary<string, string> GetProperties() => new()
        {
            { "isOn", isOn.ToString() }
        };

        protected override void SetProperties(Dictionary<string, string> properties)
        {
            isOn = bool.Parse(properties.GetValueOrDefault("isOn", "false"));
        }

    }
}