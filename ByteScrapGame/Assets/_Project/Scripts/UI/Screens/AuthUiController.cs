using System;
using _Project.Scripts.GameRoot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

namespace _Project.Scripts.UI.Screens
{
    public class AuthUiController : ScreenBase
    {
        public TMP_InputField usernameField;
        public TMP_InputField passwordField;
        public Button loginButton;
        
        public TMP_Text headerText;
        
        
        public void Show(bool isRegister = false)
        {
            loginButton.onClick.RemoveAllListeners();
            if (isRegister)
            {
                headerText.text = "Регистрация";
                loginButton.onClick.AddListener(SubmitRegister);
            }
            else
            {
                headerText.text = "Вход";
                loginButton.onClick.AddListener(SubmitLogin);
            }
            
            usernameField.text = "";
            passwordField.text = "";
            
            Open();
        }

        private void OnSuccess(long code, string json)
        {
            Bootstrap.Instance.ui.menu.UpdateUserInfo();
            Close();
        }

        private void OnError(long code, string msg)
        {
            Bootstrap.Instance.ui.popup.Show("Ошибка авторизации", msg);
        }
        
        private void SubmitLogin()
        {
            Bootstrap.Instance.api.Login(usernameField.text, passwordField.text, OnSuccess, OnError);
        }
        private void SubmitRegister()
        {
            Bootstrap.Instance.api.Register(usernameField.text, passwordField.text, OnSuccess, OnError);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }
    }
}