using System.Collections.Generic;
using _Project.Scripts.ElectricitySystem;
using Newtonsoft.Json;
using UnityEngine;

public class LevelData
{
    public string ID;
    public string name;
    public string description;
    public int difficulty;
    public string[] avalibleComponents;
    public List<GridCellData> initialGridCells;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private BuildingSystem buildingSystem;
    
    public LevelData currentLevel;


    private void InitializeLevel()
    {
        CleanUpLevel();
        
        foreach (var cell in currentLevel.initialGridCells)
        {
            buildingSystem.PlaceComponentByType(cell.component.componentType, cell);
        }
        buildingSystem.avalibleComponents = currentLevel.avalibleComponents;
        
    }

    private void CleanUpLevel()
    {
        
    }

    private LevelData LoadLevelData(string levelName)
    {
        string json = Resources.Load<TextAsset>($"Levels/{levelName}.json").text;
        
        return JsonConvert.DeserializeObject<LevelData>(json);
    }
}
