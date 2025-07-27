using System.Collections.Generic;
using _Project.Scripts.ElectricitySystem.Components;
using UnityEngine;

[System.Serializable]
public class CircuitState
{
    public List<ComponentSaveData> components = new List<ComponentSaveData>();
    
    public void CaptureState(Dictionary<Vector2Int, CircuitComponent> components)
    {
        this.components.Clear();
        foreach (var comp in components.Values)
        {
            var data = CreateSaveData(comp);
            data.gridPosition = new Vector2Int(comp.GridX, comp.GridY);
            data.CollectFromComponent(comp);
            this.components.Add(data);
        }
    }

    public void RestoreState(CircuitManager manager)
    {
        manager.ClearAllComponents();
        
        foreach (var data in components)
        {
            //TODO: Вынести регистрацию компонентов и привязки к ним префабов в отдельный класс
            
            //data.ApplyToComponent(comp);
        }
        
        manager.RequestCircuitUpdate();
    }

    // Это тоже туда
    private ComponentSaveData CreateSaveData(CircuitComponent component)
    {
        return component switch
        {
            WireComponent => new BaseSaveData(),
            SwitchComponent => new SwitchSaveData(),
            BatteryComponent => new BatterySaveData(),
            _ => new BaseSaveData()
        };
    }
}