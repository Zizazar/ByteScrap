using System;
using System.Collections.Generic;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem
{
    public class Pin : MonoBehaviour
    {
        public string Name;
        public string id;
        public CircuitComponent component;
        public bool IsInput;
        public float Voltage;
    
        public List<Pin> connectedPins = new List<Pin>();

        public void Connect(Pin otherPin)
        {
            if (connectedPins.Contains(otherPin)) return;
            
            connectedPins.Add(otherPin);
            otherPin.Connect(this);
        }
        public void SetVoltage(float newVoltage)
        {
            Voltage = newVoltage;
            foreach (var pin in connectedPins)
            {
                pin.Voltage = Voltage;
                pin.component.UpdateLogic();
            }
        }
    }
}