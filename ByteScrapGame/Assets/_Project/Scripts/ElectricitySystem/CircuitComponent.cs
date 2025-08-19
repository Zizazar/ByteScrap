using System;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.ElectricitySystem;

public enum Direction { North, South, East, West }

public abstract class CircuitComponent : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    private Material[] _cachedMaterials;
    
    public int GridX { get; private set; }
    public int GridY { get; private set; }
    
    public string ID { get; private set; }
    
    public virtual string ComponentType => GetType().Name;
    
    protected bool currentState;
    protected bool newState;
    
    private Dictionary<string, string> _properties;


    public virtual void Initialize(int gridX, int gridY)
    {
        GridX = gridX;
        GridY = gridY;
        ID = Guid.NewGuid().ToString();
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

    public ComponentData ToComponentData()
    {
        return new ComponentData
        {
            componentID = ID,
            componentType = ComponentType,
            properties = GetProperties()
        };
    }

    public void FromComponentData(ComponentData data)
    {
        ID = data.componentID;
        SetProperties(data.properties);
    }

    protected virtual Dictionary<string, string> GetProperties() => new();

    protected virtual void SetProperties(Dictionary<string, string> properties)
    {
        
    }
    
}