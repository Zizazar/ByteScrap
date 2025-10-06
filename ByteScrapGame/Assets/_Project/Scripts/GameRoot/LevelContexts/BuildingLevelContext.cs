using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.GameRoot.States.GameStates;
using _Project.Scripts.LevelAndGoals;
using _Project.Scripts.UI;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.GameRoot.LevelContexts
{
    public class BuildingLevelContext : LevelContext
    {
        public BuildingSystem buildingSystem;
        public Grid grid;
        public CircuitManager circuitManager;
        public SaveSystem saveSystem;
        
        private string _currentLevelID;
        
        public override void InitLevel(string id, SaveTypes saveType)
        {
            _currentLevelID = id;
            
            Bootstrap.Instance.sm_Game.ChangeState(new BuildingGState());
            

            // Загружаем сейв из файла если он есть
            if (SaveSystem.SaveFileExist(id))
            {
                saveSystem.LoadFromSaveFile(id);
            }
            else
            {
                var defaultLevelJson = Resources.Load<TextAsset>($"Levels/{id}");
                var levelData = JsonConvert.DeserializeObject<LevelData>(defaultLevelJson.text);
                
                buildingSystem.LoadGrid(levelData.initialGridCells);
                Bootstrap.Instance.goalSystem.LoadGoals(levelData.goals);
                
            }
            
            Bootstrap.Instance.ui.inGameOverlay.goalListController.Init(); 
            
            buildingSystem.enabled = true;
        }
        

        public override void DisposeLevel()
        {
            // Обновляем цели
            saveSystem.SaveToFile(_currentLevelID);
            circuitManager.ClearAllComponents();
            base.DisposeLevel();
        }
    }
}