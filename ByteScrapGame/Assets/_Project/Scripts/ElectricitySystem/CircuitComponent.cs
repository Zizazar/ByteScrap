using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem
{
    public abstract class CircuitComponent : MonoBehaviour
    {
        public string componentId;
        public string componentType;
        
        public List<Pin> pins = new();

        private void Start()
        {
            if (string.IsNullOrEmpty(componentId))
                componentId = Guid.NewGuid().ToString();
            Init();
        }

        public abstract void Init();
        
        public abstract void UpdateLogic();

        protected Pin GetPin(string pinName) => pins.FirstOrDefault(p => p.Name == pinName);
        protected Pin GetPin(bool isInput) => pins.FirstOrDefault(p => p.IsInput == isInput);
    }
}