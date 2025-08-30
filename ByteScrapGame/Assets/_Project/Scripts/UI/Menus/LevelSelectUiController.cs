using UnityEngine;

namespace _Project.Scripts.UI
{
    public class LevelSelectUiController : ScreenBase
    {
        [SerializeField] private GameObject CardPrefab;
        [SerializeField] private Transform CardContainer;
        
        
        public void AddLevelCard(LevelData levelData)
        {
            var cardObj = Instantiate(CardPrefab, CardContainer);

            var card =  cardObj.GetComponent<LevelSelectButtonController>();
            
            card.Init(levelData);
        }
    }
}