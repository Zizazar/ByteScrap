using System.Collections.Generic;
using System.IO;
using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.GameRoot;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class LevelSelectUiController : ScreenBase
    {
        [SerializeField] private GameObject CardPrefab;
        [SerializeField] private Transform CardContainer;
        
        private List<GameObject> Cards = new();

        public void UpdateLevelsList()
        {
            ClearLevelCards();
            
            var jsonAssets = Resources.LoadAll<TextAsset>("Levels");

            Debug.Log($"found {jsonAssets.Length} levels");
            foreach (var jsonAsset in jsonAssets)
            {
                
                var levelData = JsonConvert.DeserializeObject<LevelData>(jsonAsset.text);
                Debug.Log($"Added level {levelData.ID}");

                LevelButtonData levelButtonData = new LevelButtonData();
                levelButtonData.id = levelData.ID;
                levelButtonData.levelName = levelData.name;
                levelButtonData.description = levelData.description;
                
                if (SaveSystem.SaveFileExist(levelData.ID))
                {
                    var localJson = File.ReadAllText(SaveSystem.GetSavePath(levelData.ID));
                    levelButtonData.saveType = SaveTypes.LOCAL;
                    var localLevel = JsonConvert.DeserializeObject<SaveData>(localJson);
                    
                    levelButtonData.isCompleted = localLevel.isCompleted;
                }
                else if (false) // TODO: Проверка на облачные сохранения
                {
                    levelButtonData.saveType = SaveTypes.CLOUD;
                }
                else 
                {
                    levelButtonData.saveType = SaveTypes.NONE;
                }
                    
                
                
                AddLevelCard(levelButtonData);
                
            }
            
        }
        
        
        public void AddLevelCard(LevelButtonData levelButtonData)
        {
            var cardObj = Instantiate(CardPrefab, CardContainer);

            var card =  cardObj.GetComponent<LevelSelectButtonController>();
            
            card.Init(levelButtonData);
            Cards.Add(cardObj);
        }

        public void ClearLevelCards()
        {
            foreach (var card in Cards)
            {
                Destroy(card.gameObject);
            }
            Cards.Clear();
        }
    }
}