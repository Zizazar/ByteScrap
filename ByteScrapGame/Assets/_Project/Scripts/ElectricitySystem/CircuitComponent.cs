using System;
using UnityEngine;

public enum Direction { North, South, East, West }

[Serializable]
public abstract class ComponentSaveData
{
    public Vector2Int gridPosition;
    public abstract void ApplyToComponent(CircuitComponent component);
    public abstract void CollectFromComponent(CircuitComponent component);
}
[Serializable]
public class BaseSaveData : ComponentSaveData
{
    public Vector2Int gridPosition;
    public override void ApplyToComponent(CircuitComponent component) {}
    public override void CollectFromComponent(CircuitComponent component) {}
}

public abstract class CircuitComponent : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    private Material[] _cachedMaterials;
    
    public int GridX { get; private set; }
    public int GridY { get; private set; }
    protected bool currentState;
    protected bool newState;


    public virtual void Initialize(int gridX, int gridY)
    {
        GridX = gridX;
        GridY = gridY;
        currentState = false;
        newState = false;
    }

    public virtual void ResetState()
    {
        currentState = false;
        newState = false;
    }

    public virtual void PrepareForUpdate()
    {
        newState = false;
    }

    public virtual void UpdateState()
    {
        currentState = newState;
    }

    public abstract void ReceiveSignal(Direction fromDirection, bool signal);
    public abstract bool GetOutput(Direction direction);
}