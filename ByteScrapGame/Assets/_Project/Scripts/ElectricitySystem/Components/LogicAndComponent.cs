using System.Linq;

namespace _Project.Scripts.ElectricitySystem.Components
{
    public class LogicAndComponent : CircuitComponent
    {
        public override void Init()
        {
            componentType = "LogicAnd";
        }

        public override void UpdateLogic()
        {
            float result = 1f;
            foreach (var pin in pins.Where(pin => pin.IsInput && pin.Voltage == 0))
            {
                result = 0f;
            }
        
            foreach (var pin in pins.Where(pin => !pin.IsInput))
            {
                pin.SetVoltage(result);
            }
        }
    }
}