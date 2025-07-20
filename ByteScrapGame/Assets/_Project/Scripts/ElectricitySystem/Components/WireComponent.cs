public class WireComponent : CircuitComponent
{
    public override void ReceiveSignal(Direction fromDirection, bool signal)
    {
        if (signal) newState = true;
    }

    public override bool GetOutput(Direction direction)
    {
        return currentState;
    }
}