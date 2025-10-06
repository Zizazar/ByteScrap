using System.Collections.Generic;
using _Project.Scripts.GameRoot;
using _Project.Scripts.Player;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem.Components
{

    public class SwitchComponent : CircuitComponent, IInteractable
    {
        public bool isOn = false;
    
        [SerializeField] private Transform rotatable;
        [SerializeField] private float onRotation;
        [SerializeField] private float offRotation;
        
        public void OnInteract() {
            isOn = !isOn;
            Bootstrap.Instance.goalSystem.TriggerComponentChangeProperty(this);
            
            CircuitManager.Instance.RequestCircuitUpdate();
            UpdateVisuals();
        }

        public override void ReceiveSignal(Direction fromDirection, bool signal)
        {
            if (signal) newState = true;
        }

        public override bool GetOutput(Direction direction)
        {
            return isOn && currentState;
        }

        public override Dictionary<string, string> GetProperties() => new()
        {
            { "isOn", isOn.ToString() }
        };

        protected override void SetProperties(Dictionary<string, string> properties)
        {
            isOn = bool.Parse(properties.GetValueOrDefault("isOn", "false"));
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            var angle = isOn ? onRotation : offRotation;
            rotatable.DORotate(new Vector3(0, 0, angle), 0.5f);
        }
    }
}