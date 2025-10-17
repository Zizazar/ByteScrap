using System.Collections;
using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.ElectricitySystem.Systems.Responses;
using _Project.Scripts.GameRoot.States.GameStates;
using _Project.Scripts.LevelAndGoals;
using _Project.Scripts.UI;
using Newtonsoft.Json;
using Unity.VisualScripting;
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
        
        public override IEnumerator InitLevel(string id, SaveTypes saveType)
        {
            _currentLevelID = id;

            switch (saveType)
            {
                case SaveTypes.NONE:
                {
                    var defaultLevelJson = Resources.Load<TextAsset>($"Levels/{id}");
                    var levelData = JsonConvert.DeserializeObject<LevelData>(defaultLevelJson.text);
                
                    buildingSystem.LoadGrid(levelData.initialGridCells);
                    Bootstrap.Instance.goalSystem.LoadGoals(levelData.goals);
                    break;
                }
                case SaveTypes.LOCAL:
                {
                    saveSystem.LoadFromSaveFile(id);
                    break;
                }
                case SaveTypes.CLOUD:
                {
                    SaveDataResponse response = null;
                    
                    
                    yield return Bootstrap.Instance.api.GetSave(id, (code, resp) =>
                    {
                        response = resp;
                    }, (code, err) =>
                    {
                        Bootstrap.Instance.sm_Game.ChangeState(new MenuGState());
                    });

                    if (response is null) yield break;

                    var levelData = JsonConvert.DeserializeObject<SaveData>(response.data);
                    buildingSystem.LoadGrid(levelData.gridData);
                    Bootstrap.Instance.goalSystem.LoadGoals(levelData.goals);
                    break;
                }
            }
            
            Bootstrap.Instance.sm_Game.ChangeState(new BuildingGState());
            Bootstrap.Instance.ui.inGameOverlay.goalListController.Init(); 
            
            buildingSystem.enabled = true;
        }
        

        public override IEnumerator DisposeLevel()
        {
            var json = saveSystem.SaveToJson();
            // Если пользователь авторизован то загружаем в одлако
            yield return Bootstrap.Instance.api.UploadSave(_currentLevelID, json, null, (code, msg) =>
            {
                //fallback
                saveSystem.SaveToFile(_currentLevelID); // TODO: Сделать нормальную загрузку после выхода.
            });
            
            yield return null;
            Bootstrap.Instance.sm_Game.ChangeState(new MenuGState());
        }
    }
}