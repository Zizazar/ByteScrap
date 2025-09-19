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
        
        public LevelData currentLevel;
        
        public override void InitLevel(LevelData levelData)
        {
            Bootstrap.Instance.sm_Game.ChangeState(new BuildingGState());
            
            currentLevel = levelData;
            buildingSystem.avalibleComponents = currentLevel.avalibleComponents;

            // Загружаем сейв из файла если он есть
            if (saveSystem.SaveFileExist(levelData.ID))
            {
                saveSystem.LoadFromSaveFile(levelData.ID);
            }
            else
            {
                buildingSystem.LoadGrid(currentLevel.initialGridCells);
                Bootstrap.Instance.goalSystem.LoadGoals(levelData.goals);
                
            }
            
            buildingSystem.enabled = true;
            
            
            
            base.InitLevel(levelData);
        }
        

        public override void DisposeLevel()
        {
            currentLevel.goals = Bootstrap.Instance.goalSystem.GetGoals(); // Обновляем цели
            saveSystem.SaveToFile(currentLevel.ID);
            circuitManager.ClearAllComponents();
            base.DisposeLevel();
        }
    }
}