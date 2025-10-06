using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.States.GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{

    public enum SaveTypes
    {
        NONE,
        LOCAL,
        CLOUD
    }

    public class LevelButtonData
    {
        public string id;
        public SaveTypes saveType;
        public bool isCompleted;
        public string levelName;
        public string description;
    }
    public class LevelSelectButtonController : MonoBehaviour
    {
        [Header("sprites")]
        public Sprite localSaveIcon;
        public Sprite cloudSaveIcon;
        
        [Header("References")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button button;
        [SerializeField] private Image saveIcon;
        [SerializeField] private Image completeIcon;
        
        private LevelButtonData _data;

        public void Init(LevelButtonData data)
        {
            _data = data;
            
            nameText.text = data.levelName;
            descriptionText.text = data.description;
            button.onClick.AddListener(ButtonClicked);

            saveIcon.sprite = data.saveType switch
            {
                SaveTypes.LOCAL => localSaveIcon,
                SaveTypes.CLOUD => cloudSaveIcon,
                _ => null
            };
            saveIcon.gameObject.SetActive(data.saveType != SaveTypes.NONE);
            
            completeIcon.gameObject.SetActive(data.isCompleted);
        }
        

        private void ButtonClicked()
        {
            Bootstrap.Instance.sm_Game.ChangeState(new LoadingLevelState(_data.id, _data.saveType));
        }
    }
}