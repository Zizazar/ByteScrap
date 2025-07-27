using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem.Components
{
[System.Serializable]
    public class SwitchSaveData : ComponentSaveData
    {
        public bool isOn;

        public override void ApplyToComponent(CircuitComponent component)
        {
            if (component is SwitchComponent switchComp)
            {
                switchComp.isOn = isOn;
            }
        }

        public override void CollectFromComponent(CircuitComponent component)
        {
            if (component is SwitchComponent switchComp)
            {
                isOn = switchComp.isOn;
            }
        }
    }

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
    }
}