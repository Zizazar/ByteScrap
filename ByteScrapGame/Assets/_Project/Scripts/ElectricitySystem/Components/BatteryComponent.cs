[System.Serializable]
public class BatterySaveData : ComponentSaveData
{
    public override void ApplyToComponent(CircuitComponent component) { }
    public override void CollectFromComponent(CircuitComponent component) { }
}

public class BatteryComponent : CircuitComponent
{
    public override void Initialize(int gridX, int gridY)
    {
        base.Initialize(gridX, gridY);
        currentState = true;
        newState = true;
    }

    public override void ResetState()
    {
        currentState = true;
        newState = true;
    }
    
    public override void PrepareForUpdate()
    {
        // Не сбрасываем состояние батареи!
        newState = true; // Всегда остается включенной
    }

    public override void ReceiveSignal(Direction fromDirection, bool signal)
    {
        
    }

    public override bool GetOutput(Direction direction)
    {
        return true; 
    }
}