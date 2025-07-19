using System.Collections.Generic;
using _Project.Scripts.ElectricitySystem;
using UnityEngine;

public class CircuitManager : MonoBehaviour
{
    public static CircuitManager Instance;
    public List<CircuitComponent> components = new List<CircuitComponent>();
    public List<PinConnection> connections = new List<PinConnection>();

    private void Awake() => Instance = this;

    public void AddConnection(Pin outputPin, Pin inputPin)
    {
        if (outputPin.IsInput || !inputPin.IsInput) return;

        var connection = new PinConnection(outputPin, inputPin);
        connections.Add(connection);
        outputPin.connectedPins.Add(inputPin);
        inputPin.connectedPins.Add(outputPin);
    }

    public CircuitData GetSaveData()
    {
        var data = new CircuitData();
        
        foreach (var comp in components)
        {
            data.components.Add(new ComponentData()
            {
                id = comp.componentId,
                type = comp.componentType,
                position = comp.transform.position,
                rotation = comp.transform.eulerAngles
            });
        }

        foreach (var conn in connections)
        {
            data.connections.Add(new ConnectionData()
            {
                startPin = new PinData()
                {
                    id = conn.output.id,
                    componentId = conn.output.component.componentId,
                    isInput = false
                },
                endPin = new PinData()
                {
                    id = conn.input.id,
                    componentId = conn.input.component.componentId,
                    isInput = true
                }
            });
        }
        
        return data;
    }

    public void LoadFromData(CircuitData data)
    {
        ClearCircuit();
        
        // Создаем компоненты
        Dictionary<string, CircuitComponent> compDict = new Dictionary<string, CircuitComponent>();
        foreach (var compData in data.components)
        {
            var comp = CreateComponent(compData);
            if (!comp) continue;
            compDict.Add(compData.id, comp);
        }

        // Восстанавливаем соединения
        foreach (var connData in data.connections)
        {
            if (compDict.TryGetValue(connData.startPin.componentId, out var startComp) &&
                compDict.TryGetValue(connData.endPin.componentId, out var endComp))
            {
                Pin outputPin = startComp.pins.Find(p => p.id == connData.startPin.id);
                Pin inputPin = endComp.pins.Find(p => p.id == connData.endPin.id);
                
                if (outputPin != null && inputPin != null)
                    AddConnection(outputPin, inputPin);
            }
        }
    }

    void ClearCircuit()
    {
        // Очистка схемы
    }

    private static GameObject GetPrefab(string type)
    {
        return Resources.Load<GameObject>($"Components/C_{type}");
    }

    public CircuitComponent CreateComponent(ComponentData compData)
    {
        var prefab = GetPrefab(compData.type);
        if (prefab == null) return null;
            
        var newComp = Instantiate(prefab, compData.position, Quaternion.Euler(compData.rotation));
        var comp = newComp.GetComponent<CircuitComponent>();
        comp.componentId = compData.id;
        comp.componentType = compData.type;
        components.Add(comp);
        return comp;
    }
    
}

public class PinConnection
{
    public Pin output;
    public Pin input;

    public PinConnection(Pin outPin, Pin inPin)
    {
        output = outPin;
        input = inPin;
    }
}