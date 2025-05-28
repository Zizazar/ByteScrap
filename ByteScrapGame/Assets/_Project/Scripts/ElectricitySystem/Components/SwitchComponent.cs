using System;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem.Components
{
    public class SwitchComponent : CircuitComponent, IInteractable
    {
        private bool isOn;

        private void Start()
        {
            Init();
        }

        public override void Init()
        {
            base.Init();
            GetPin("Output").Type = PinType.Output;
        }

        protected override void HandlePinValueChanged(Pin updatedPin)
        {
            // Переключатель не имеет входных пинов
        }

        public void OnInteract()
        {
            Debug.Log("toggled!");
            isOn = !isOn;
            GetPin("P_Output").UpdateVoltage(isOn ? 5f : 0f);
        }
    }
}