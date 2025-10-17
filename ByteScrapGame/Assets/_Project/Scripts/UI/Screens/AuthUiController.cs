using _Project.Scripts.GameRoot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            
            Open();
        }

        private void SubmitLogin()
        {
            Bootstrap.Instance.api.Login(usernameField.text, passwordField.text, 
                (code, msg) => Debug.Log('d')
                );
        }
        private void SubmitRegister()
        {
            Bootstrap.Instance.api.Login(usernameField.text, passwordField.text);
        }
    }
}