using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CircuitManager : MonoBehaviour
{
    private Dictionary<Vector2Int, CircuitComponent> components = new();
    
    private Queue<CircuitComponent> updateQueue = new();
    private HashSet<Vector2Int> scheduledForUpdate = new HashSet<Vector2Int>();
    private bool isUpdating;
    private bool updateRequested;

    #region Singleton
    public static CircuitManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
    
    public void RegisterComponent(CircuitComponent component, int gridX, int gridY)
    {
        Vector2Int pos = new Vector2Int(gridX, gridY);
        components[pos] = component;
        RequestCircuitUpdate();
    }
    
    public void ClearAllComponents() => components.Clear();

    public bool TryRemoveComponent(Vector2Int gridPos)
    {
        components.TryGetValue(gridPos, out CircuitComponent component);
        if (!component) return false; // Если на этом месте нет компонента
        
        Destroy(component.gameObject);
        components.Remove(gridPos);
        RequestCircuitUpdate();
        return true;
    }

    public void RequestCircuitUpdate()
    {
        if (isUpdating)
        {
            updateRequested = true;
            return;
        }
        
        StartCoroutine(UpdateCircuitCoroutine());
    }

    private IEnumerator UpdateCircuitCoroutine()
    {
        // Нет необходимости обновлять пустую схему
        if (components.Count == 0) yield break;
        
        isUpdating = true;
        
        // Фаза 1: Подготовка компонентов
        foreach (var component in components.Values)
        {
            component.PrepareForUpdate();
            if (component is BatteryComponent)
            {
                updateQueue.Enqueue(component);
                scheduledForUpdate.Add(new Vector2Int(component.GridX, component.GridY));
            }
        }

        // Фаза 2: Распространение сигналов
        int safetyCounter = 0;
        int maxIterations = components.Count * 10;
        
        while (updateQueue.Count > 0 && safetyCounter < maxIterations)
        {
            safetyCounter++;
            CircuitComponent current = updateQueue.Dequeue();
            Vector2Int currentPos = new Vector2Int(current.GridX, current.GridY);
            scheduledForUpdate.Remove(currentPos);

            // Обрабатываем только если компонент все еще существует
            if (components.ContainsKey(currentPos) && components[currentPos] == current)
            {
                PropagateSignal(current, Direction.North);
                PropagateSignal(current, Direction.South);
                PropagateSignal(current, Direction.East);
                PropagateSignal(current, Direction.West);
            }

            // Делаем паузу каждые 50 обработанных компонентов
            if (safetyCounter % 50 == 0)
            {
                yield return null;
            }
        }

        // Фаза 3: Применение состояний
        foreach (var component in components.Values)
        {
            component.UpdateState();
        }

        isUpdating = false;
        
        // Запускаем ещё раз если надо
        if (updateRequested)
        {
            updateRequested = false;
            StartCoroutine(UpdateCircuitCoroutine());
        }
        
        if (safetyCounter >= maxIterations)
        {
            Debug.LogError("Circuit update iteration limit exceeded! Possible infinite loop.");
        }
    }

    private void PropagateSignal(CircuitComponent source, Direction direction)
    {
        bool signal = source.GetOutput(direction);
        if (!signal) return;

        Vector2Int neighborPos = GetNeighborPosition(source.GridX, source.GridY, direction);
        
        if (!components.TryGetValue(neighborPos, out CircuitComponent neighbor)) 
            return;
        
        // Передаем сигнал только если компонент существует
        if (!neighbor) return;
        neighbor.ReceiveSignal(GetOppositeDirection(direction), signal);
            
        if (!scheduledForUpdate.Contains(neighborPos))
        {
            updateQueue.Enqueue(neighbor);
            scheduledForUpdate.Add(neighborPos);
        }
    }

    public bool IsPositionOccupied(Vector2Int gridPos)
    {
        return components.ContainsKey(gridPos);
    }

    private static Vector2Int GetNeighborPosition(int x, int y, Direction direction)
    {
        return direction switch
        {
            Direction.North => new Vector2Int(x, y + 1),
            Direction.South => new Vector2Int(x, y - 1),
            Direction.East => new Vector2Int(x + 1, y),
            Direction.West => new Vector2Int(x - 1, y),
            _ => new Vector2Int(x, y)
        };
    }

    private static Direction GetOppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.South,
            Direction.South => Direction.North,
            Direction.East => Direction.West,
            Direction.West => Direction.East,
            _ => direction
        };
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, 200, 300));
        GUILayout.Label($"Components: {components.Count}");
        GUILayout.Label($"Queue: {updateQueue.Count}");
        GUILayout.Label($"Updating: {isUpdating}");
        GUILayout.EndArea();
    }

    public Queue<CircuitComponent> GetUpdateQueue()
    {
        return updateQueue;
    }
}