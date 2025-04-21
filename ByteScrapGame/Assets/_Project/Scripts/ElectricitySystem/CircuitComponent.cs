using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem
{
    public abstract class CircuitComponent : MonoBehaviour
    {
        protected Dictionary<string, Pin> pins = new Dictionary<string, Pin>();

        public virtual void Init()
        {
            foreach (var pin in GetComponentsInChildren<Pin>())
            {
                pins.Add(pin.name, pin);
                pin.OnValueChanged += HandlePinValueChanged;
            }
        }

        protected abstract void HandlePinValueChanged(Pin updatedPin);
    
        public Pin GetPin(string pinName) => pins.TryGetValue(pinName, out var pin) ? pin : null;
    }
}