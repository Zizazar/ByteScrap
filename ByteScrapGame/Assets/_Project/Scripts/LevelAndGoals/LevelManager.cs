using System.Collections.Generic;
using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.LevelAndGoals;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class LevelData
{
    public string ID;
    public string name;
    public string description;
    public int difficulty;
    public string[] avalibleComponents;
    public List<GridCellData> initialGridCells;
    public List<Goal> goals;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private BuildingSystem buildingSystem;
    [SerializeField] private SaveSystem saveSystem;
    
    


    private void InitializeLevel()
    {
        
    }

    private void CleanUpLevel()
    {
        
    }
}
