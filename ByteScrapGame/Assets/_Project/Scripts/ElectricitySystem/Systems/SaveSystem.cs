using System;
using System.Collections.Generic;
using System.IO;
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
        private string GetSavePath(string id)
        {
            var saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(saveDirectory)) 
                Directory.CreateDirectory(saveDirectory);
            return Path.Combine(saveDirectory, $"{id}.json");
        }

        [SerializeField] private BuildingSystem buildingSystem;

        private string _lastJson;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _lastJson = SaveToJson();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadFromJson(_lastJson);
            }
        }

        public void LoadFromJson(string json)
        {
            var saveData = JsonConvert.DeserializeObject<SaveData>(json);
            
            LoadGrid(saveData.gridData);
        
            Debug.Log($"Схема загружена: \n {json}");
        }

        public void LoadGrid(List<GridCellData> gridData)
        {
            foreach (var data in gridData)
            {
                if (buildingSystem.circuitManager.IsPositionOccupied(new Vector2Int(data.x, data.y)))
                {
                    Debug.LogError($"Не удалось поставить компонент {data.component.componentType}({data.component.componentID}) в ({data.x}, {data.y})");
                    return;
                }
                buildingSystem.PlaceComponentByType(data.component.componentType, data);
            }
            buildingSystem.circuitManager.RequestCircuitUpdate();
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

            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented, 
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        
            Debug.Log($"Схема сохранена: \n {json}");
            return json;
        }

        public bool SaveFileExist(string id)
        {
            return File.Exists(GetSavePath(id));
        }
        
        public void LoadFromSaveFile(string id)
        {
            var json = File.ReadAllText(GetSavePath(id));
            LoadFromJson(json);
        }

        public void SaveToFile(string id)
        {
            File.WriteAllText(GetSavePath(id), SaveToJson());
        } 
    }
}