using System;
using UnityEngine;

public enum Direction { North, South, East, West }

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