using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class LevelSelectGState : IState
    {
        public void Enter()
        {
            Bootstrap.Instance.ui.levelSelect.Open();
            UpdateLevelsList();
        }

        public void Exit()
        {
            Bootstrap.Instance.ui.levelSelect.Close();
            

        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        private void UpdateLevelsList()
        {
            Bootstrap.Instance.ui.levelSelect.ClearLevelCards();
            
            var jsonAssets = Resources.LoadAll<TextAsset>("Levels");

            Debug.Log($"found {jsonAssets.Length} levels");
            foreach (var jsonAsset in jsonAssets)
            {
                var levelData = JsonConvert.DeserializeObject<LevelData>(jsonAsset.text);
                Debug.Log($"Added level {levelData.ID}");
                Bootstrap.Instance.ui.levelSelect.AddLevelCard(levelData);
                
            }
            
        }
        
    }
}