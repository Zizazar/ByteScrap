using System;
using System.Collections.Generic;
using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.GameRoot;
using _Project.Scripts.LevelAndGoals;
using DG.Tweening;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public GridLayout grid;
    public CircuitManager circuitManager;

    public string[] avalibleComponents;
    
    private GameObject componentToPlace;
    private HologramEffect componentToPlaceHologram;
    private Vector2Int lastGridPos;
    
    
    public void UpdateHologram()
    {
        if (!componentToPlace || !componentToPlaceHologram) return;
        
        var gridPos = GetGridPos();
        if (gridPos != lastGridPos)
        {
            lastGridPos = gridPos;
            componentToPlaceHologram.SetValidState(!circuitManager.IsPositionOccupied(gridPos));
            componentToPlaceHologram.transform.DOMove(GetWorldPosFromCell(gridPos), 0.2f).SetEase(Ease.OutQuad);
        }
    }
    

    public void TryPlaceComponent()
        {
            if (!componentToPlace) return;

            var gridPos = GetGridPos();

            if (circuitManager.IsPositionOccupied(gridPos))
            {
                Debug.LogWarning("Trying to place a component on other");
                return;
            }
            
            RemoveHologram();
            
            CircuitComponent component = componentToPlace.GetComponent<CircuitComponent>();
            component.Initialize(gridPos.x, gridPos.y);
            circuitManager.RegisterComponent(component, gridPos.x, gridPos.y);
            
            Bootstrap.Instance.goalSystem.TriggerComponentPlace(component);

            componentToPlace = null;
            
            circuitManager.RequestCircuitUpdate();
        }

    public void TryRemoveComponent()
    {
        Vector3Int cellPos = grid.WorldToCell(GetMouseWorldPosition());
        Vector2Int gridPos = new Vector2Int(cellPos.x, cellPos.z); // vec3: x z - вертикальные оси | vec2: x y - вертикальные оси

        if (!circuitManager.TryRemoveComponent(gridPos)) return;
        
        // Найти и уничтожить игровой объект
        foreach (Transform child in transform)
        {
            var comp = child.GetComponent<CircuitComponent>();
            if (comp && comp.GridX == gridPos.x && comp.GridY == gridPos.y)
            {
                Destroy(child.gameObject);
                break;
            }
        }
        circuitManager.RequestCircuitUpdate();
    }

    public void RemoveHologram()
    {
        if (!componentToPlace) return;
        Destroy(componentToPlaceHologram);
        componentToPlaceHologram = null;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hit, 50, LayerMask.GetMask("Ground")) ? hit.point : Vector3.zero;
    }

    private Vector2Int GetGridPos()
    {
        Vector3Int cellPos = grid.WorldToCell(GetMouseWorldPosition());
        // vec3: x z - вертикальные оси | vec2: x y - вертикальные оси
        return new Vector2Int(cellPos.x, cellPos.z); 
    }
    private Vector3 GetWorldPosFromCell(Vector2Int gridPos)
    {
        Vector3 pos = grid.CellToWorld(new Vector3Int(gridPos.x, 0, gridPos.y));
        Vector3 offset = grid.cellSize/2;
        pos += new Vector3(offset.x, 0, offset.z);
        return pos;
    }

    public void SelectComponent(string typeName)
    {
        if (componentToPlace) Destroy(componentToPlace);
        
        componentToPlace = Instantiate(
            GetComponentPrefabByType(typeName),
            GetWorldPosFromCell(GetGridPos()),
            Quaternion.identity
        );
        componentToPlaceHologram = componentToPlace.GetComponent<HologramEffect>();
        componentToPlaceHologram.ActivateHologramEffect();
        Debug.Log($"Selected component: {componentToPlace.name}");
    }

    public void PlaceComponentByType(string typeName, GridCellData data)
    {
        var go = GetComponentPrefabByType(typeName);

        if (!go) {
            Debug.LogError($"Can't find component type: {typeName}");
            return;
        }
        
        Vector3 pos = GetWorldPosFromCell(new Vector2Int(data.x, data.y));
        
        var comp = Instantiate(
                go, pos, Quaternion.identity
                ).GetComponent<CircuitComponent>();
        // Засовываем все нужные данные в компонент
        comp.Initialize(data.x, data.y);
        comp.FromComponentData(data.component);
        
        circuitManager.RegisterComponent(comp, data.x, data.y);
        
    }

    private GameObject GetComponentPrefabByType(string typeName) =>
        Resources.Load<GameObject>($"Components/{typeName}");
    
    public void LoadGrid(List<GridCellData> gridData)
    {
        foreach (var data in gridData)
        {
            if (circuitManager.IsPositionOccupied(new Vector2Int(data.x, data.y)))
            {
                Debug.LogError($"Не удалось поставить компонент {data.component.componentType}({data.component.componentID}) в ({data.x}, {data.y})");
                return;
            }
            PlaceComponentByType(data.component.componentType, data);
        }
        circuitManager.RequestCircuitUpdate();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var pos = GetMouseWorldPosition();
            Gizmos.DrawWireSphere(pos, 0.1f);
            var gridPos = grid.WorldToCell(pos);
            Handles.Label(new Vector3(gridPos.x, 1, gridPos.z), gridPos.ToString());
        }
#endif
}