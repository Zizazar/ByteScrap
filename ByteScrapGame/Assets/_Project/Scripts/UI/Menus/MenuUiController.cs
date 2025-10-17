
using System;
using _Project.Scripts.ElectricitySystem.Systems.Responses;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.States.GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class MenuUiController : ScreenBase
    {
        
        [SerializeField] private  Button selectLevelButton;
        [SerializeField] private  Button exitButton;
        
        [SerializeField] private TMP_Text usernameText;

        
        [SerializeField] private string usernamePrefix = "Пользователь: ";
        [SerializeField] private string usernameError = "Войдите в аккаунт";
        [SerializeField] private string offlineError = "Нет подключения к серверу";
        
        private void Awake()
        {
            if (selectLevelButton == null) {
                Debug.LogError("Start Button not set");
                return;
            }
            if (exitButton == null) {
                Debug.LogError("Exit Button not set");
                return;
            }
            
            selectLevelButton.onClick.AddListener(SelectLevel);
            exitButton.onClick.AddListener(Application.Quit);

        }

        public override void Open()
        {
            UpdateUserInfo();
            base.Open();
        }

        private void SelectLevel()
        {
            // TODO: Открытие выбора уровня
            Bootstrap.Instance.sm_Game.ChangeState(new LevelSelectGState());
            //Bootstrap.Instance.sm_Game.ChangeState(new BuildingGState()); // LevelSelectGState
            //Bootstrap.Instance.ui.open
        }

        private void UpdateUserInfo()
        {
            Bootstrap.Instance.api.GetCurrentUser((code, json) =>
                {
                    var data = new UserMetaResponse().FromJson(json);
                    usernameText.text = usernamePrefix + data.name;
                }, (code, msg) =>
            {
                usernameText.text = (code == 0) ? offlineError : usernameError;
            });
        }
        
        
    }
}