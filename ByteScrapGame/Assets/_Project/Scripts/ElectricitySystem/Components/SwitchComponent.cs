namespace _Project.Scripts.ElectricitySystem.Components
{
    public class SwitchComponent : CircuitComponent
    {
        private bool isOn;

        public override void Init()
        {
            base.Init();
            GetPin("Output").Type = PinType.Output;
        }

        public void Toggle()
        {
            isOn = !isOn;
            GetPin("Output").UpdateVoltage(isOn ? 5f : 0f);
        }

        protected override void HandlePinValueChanged(Pin updatedPin)
        {
            // Переключатель не имеет входных пинов
        }
    }
}