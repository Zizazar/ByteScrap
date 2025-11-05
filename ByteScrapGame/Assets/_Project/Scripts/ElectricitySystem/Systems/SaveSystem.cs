using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Project.Scripts.GameRoot;
using _Project.Scripts.LevelAndGoals;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem
{
    [Serializable]
    public class ComponentData
    {
        public string componentID;
        public string componentType;
        public Dictionary<string, string> properties; 
    }
    
    [Serializable]
    public class SaveData
    {
        public List<GridCellData> gridData = new();
        public List<Goal> goals = new();
        public bool isCompleted;
    }

    [Serializable]
    public class GridCellData
    {
        public int x;
        public int y;
        public ComponentData component;
    }
    
    public class SaveSystem : MonoBehaviour
    {
        public static string GetSavePath(string id)
        {
            var saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(saveDirectory)) 
                Directory.CreateDirectory(saveDirectory);
            return Path.Combine(saveDirectory, $"{id}.json");
        }

        public static void DeleteLocalSaves()
        {
            string[] files = Directory.GetFiles(Path.Combine(Application.persistentDataPath, "Saves"));
            
            foreach (string file in files)
            {
                File.Delete(file);
                Debug.Log($"Удален файл: {file}");
            }
        }

        [SerializeField] private BuildingSystem buildingSystem;

        public void LoadFromJson(string json)
        {
            var saveData = JsonConvert.DeserializeObject<SaveData>(json);
            
            buildingSystem.LoadGrid(saveData.gridData);
            Bootstrap.Instance.goalSystem.LoadGoals(saveData.goals);
        
            Debug.Log($"Схема загружена: \n {json}");
        }

        public List<GridCellData> SaveGrid()
        {
            var saveData = buildingSystem.circuitManager.components.Select(
                item => 
                    new GridCellData()
                    {
                        x = item.Key.x, y = item.Key.y, component = item.Value.ToComponentData() 
                        
                    }).ToList();
            return saveData;
        }

        public string SaveToJson()
        {
            SaveData saveData = new SaveData();

            foreach (var item in buildingSystem.circuitManager.components)
            {
                saveData.gridData.Add(new GridCellData()
                {
                    x = item.Key.x,
                    y = item.Key.y,
                    component = item.Value.ToComponentData()
                });
            }
            saveData.goals = Bootstrap.Instance.goalSystem.GetGoals(); // Обновляем цели
            saveData.isCompleted = Bootstrap.Instance.goalSystem.IsAllGoalsCompleted();
            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented, 
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                });
        
            Debug.Log($"Схема сохранена: \n {json}");
            return json;
        }

        public static bool SaveFileExist(string id) => File.Exists(GetSavePath(id));
        
        
        public void LoadFromSaveFile(string id)
        {
            var json = File.ReadAllText(GetSavePath(id));
            LoadFromJson(json);
            Debug.Log($"Level {id} has been loaded");
        }

        public void SaveToFile(string id)
        {
            File.WriteAllText(GetSavePath(id), SaveToJson());
        } 
    }
}