namespace _Project.Scripts.ElectricitySystem.Components
{
    public class LogicAndComponent : CircuitComponent
    {
        public override void Init()
        {
            base.Init();
            GetPin("Input1").Type = PinType.Input;
            GetPin("Input2").Type = PinType.Input;
            GetPin("Output").Type = PinType.Output;
        }

        protected override void HandlePinValueChanged(Pin updatedPin)
        {
            float in1 = GetPin("Input1").Voltage;
            float in2 = GetPin("Input2").Voltage;
            bool output = in1 > 2.5f && in2 > 2.5f;

            GetPin("Output").UpdateVoltage(output ? 5f : 0f);
        }
    }
}