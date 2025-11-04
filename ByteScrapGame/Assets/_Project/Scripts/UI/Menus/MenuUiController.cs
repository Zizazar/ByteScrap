
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
        [SerializeField] private  Button creativeButton;
        [SerializeField] private  Button loginButton;
        [SerializeField] private  Button registerButton;
        [SerializeField] private  Button settingsButton;
        
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
            loginButton.onClick.AddListener(OpenLoginMenu);
            registerButton.onClick.AddListener(OpenRegMenu);
            creativeButton.onClick.AddListener(OpenCreative);
            settingsButton.onClick.AddListener(OpenSettings);
        }

        private void OpenSettings()
        {
            Bootstrap.Instance.ui.settings.Open();
        }

        private void OpenCreative()
        {
            Bootstrap.Instance.sm_Game.ChangeState(new LoadingLevelState("creative", SaveTypes.NONE));
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

        private void OpenLoginMenu()
        {
            Bootstrap.Instance.ui.auth.Show(false);
        }
        private void OpenRegMenu()
        {
            Bootstrap.Instance.ui.auth.Show(true);
        }

        private void SetAuthButtonsActive(bool isActive)
        {
            loginButton.gameObject.SetActive(isActive);
            registerButton.gameObject.SetActive(isActive);
        }

        public void UpdateUserInfo()
        {
            Bootstrap.Instance.api.GetCurrentUser((code, json) =>
                {
                    var data = new UserMetaResponse().FromJson(json);
                    usernameText.text = usernamePrefix + data.name;
                    SetAuthButtonsActive(false);
                }, (code, msg) =>
            {
                if (code == 0)
                {
                    usernameText.text = offlineError;
                    SetAuthButtonsActive(false);
                }
                else
                {
                    usernameText.text = usernameError;
                    SetAuthButtonsActive(true);
                }
                
            });
        }
        
        
        
        
    }
}