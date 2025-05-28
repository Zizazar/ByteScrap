using System;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem.Components
{
    public class LightComponent : CircuitComponent
    {
        private Light _light;
        
        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        protected override void HandlePinValueChanged(Pin updatedPin)
        {
            if (updatedPin.Type == PinType.Input) _light.intensity = updatedPin.Voltage;
        }
    }
}