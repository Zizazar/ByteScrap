using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public GridLayout grid;
    public Camera mainCamera;
    public CircuitManager circuitManager;
    public GameObject[] componentPrefabs;
    
    private int selectedComponentIndex;
    
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

            Destroy(componentToPlaceHologram);
            
            
            CircuitComponent component = componentToPlace.GetComponent<CircuitComponent>();
            component.Initialize(gridPos.x, gridPos.y);
            circuitManager.RegisterComponent(component, gridPos.x, gridPos.y);
            
            componentToPlace = null;
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
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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

    public void SelectComponent(int index)
    {
        selectedComponentIndex = index;
        Destroy(componentToPlace);
        Destroy(componentToPlaceHologram);
        componentToPlace = Instantiate(
            componentPrefabs[selectedComponentIndex],
            GetWorldPosFromCell(GetGridPos()),
            Quaternion.identity
        );
        componentToPlaceHologram = componentToPlace.GetComponent<HologramEffect>();
        componentToPlaceHologram.ActivateHologramEffect();
        Debug.Log($"Selected component: {componentToPlace.name}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var pos = GetMouseWorldPosition();
        Gizmos.DrawWireSphere(pos, 0.1f);
        var gridPos = grid.WorldToCell(pos);
        Handles.Label(new Vector3(gridPos.x, 1, gridPos.z), gridPos.ToString());
    }
}