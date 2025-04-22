
using System;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.States.GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class MenuUiController : ScreenBase
    {
        public Button _selectLevelButton;
        public Button _exitButton;

        private void Awake()
        {
            if (_selectLevelButton == null) {
                Debug.LogError("Start Button not set");
                return;
            }
            if (_exitButton == null) {
                Debug.LogError("Exit Button not set");
                return;
            }
            
            _selectLevelButton.onClick.AddListener(SelectLevel);
            _exitButton.onClick.AddListener(Application.Quit);

        }

        private void SelectLevel()
        {
            // TODO: Открытие выбора уровня
            Bootstrap.Instance.sm_Game.ChangeState(new LevelSelectGState());
            //Bootstrap.Instance.ui.open
        }
        
        
    }
}