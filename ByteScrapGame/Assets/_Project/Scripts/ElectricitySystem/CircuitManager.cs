using System.Collections.Generic;
using _Project.Scripts.ElectricitySystem.Components;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem
{
    public class CircuitManager
    {
        private List<CircuitComponent> components = new List<CircuitComponent>();

        public void Init()
        {
        }
        
        public void RegisterComponent(CircuitComponent component)
        {
            component.Init();
            components.Add(component);
        }

        public void ConnectPins(Pin a, Pin b)
        {
            a.Connect(b);
        }
    }
}