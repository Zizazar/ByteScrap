using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class LevelSelectUiController : ScreenBase
    {
        [SerializeField] private GameObject CardPrefab;
        [SerializeField] private Transform CardContainer;
        
        private List<GameObject> Cards = new();
        
        public void AddLevelCard(LevelData levelData)
        {
            var cardObj = Instantiate(CardPrefab, CardContainer);

            var card =  cardObj.GetComponent<LevelSelectButtonController>();
            
            card.Init(levelData);
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