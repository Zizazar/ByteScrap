using System;
using System.Collections.Generic;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem
{
    public class Pin : MonoBehaviour
    {
        public event Action<Pin> OnValueChanged;
    
        public CircuitComponent Owner { get; set; }
        public PinType Type;
        public float Voltage;
    
        private List<Pin> _connectedPins = new List<Pin>();

        public void Connect(Pin otherPin)
        {
            if (!_connectedPins.Contains(otherPin))
            {
                _connectedPins.Add(otherPin);
                otherPin.Connect(this);
            }
        }

        public void UpdateVoltage(float newVoltage)
        {
            if (Mathf.Approximately(Voltage, newVoltage)) return;
        
            Voltage = newVoltage;
            OnValueChanged?.Invoke(this);
        
            foreach (var pin in _connectedPins)
            {
                pin.UpdateVoltage(newVoltage);
            }
        }
    }

    public enum PinType
    {
        Input,
        Output
    }
}