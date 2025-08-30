using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.States.GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class LevelSelectButtonController : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button button;
        
        private LevelData _levelData;

        public void Init(LevelData levelData)
        {
            _levelData = levelData;
            
            nameText.text = levelData.name;
            descriptionText.text = levelData.description;
            button.onClick.AddListener(ButtonClicked);

        }

        private void ButtonClicked()
        {
            Bootstrap.Instance.sm_Game.ChangeState(new LoadingLevelState(_levelData));
        }
    }
}