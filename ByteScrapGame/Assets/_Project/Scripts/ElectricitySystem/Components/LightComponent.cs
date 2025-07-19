using System;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem.Components
{
    public class LightComponent : CircuitComponent
    {
        private Light _light;
        

        public override void Init()
        {
            componentType = "Light";
            _light = GetComponent<Light>();
        }

        public override void UpdateLogic()
        {
            _light.intensity = GetPin(isInput: true).Voltage > 5 ? 1 : 0;
        }
    }
}