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
            var jsonAssets = Resources.LoadAll<TextAsset>("Levels");

            foreach (var jsonAsset in jsonAssets)
            {
                var levelData = JsonConvert.DeserializeObject<LevelData>(jsonAsset.text);
                
                Bootstrap.Instance.ui.levelSelect.AddLevelCard(levelData);
                
            }
            
        }
        
    }
}