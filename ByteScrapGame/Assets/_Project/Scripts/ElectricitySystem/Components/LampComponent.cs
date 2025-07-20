using UnityEngine;

public class LampComponent : CircuitComponent
{
    [SerializeField] private Light bulbLight;

    private void Start() => UpdateVisual();

    public override void UpdateState()
    {
        base.UpdateState();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        bulbLight.enabled = currentState;
    }

    public override void ReceiveSignal(Direction fromDirection, bool signal)
    {
        if (signal) newState = true;
    }

    public override bool GetOutput(Direction direction)
    {
        return false; // Лампа не передает сигнал
    }
}