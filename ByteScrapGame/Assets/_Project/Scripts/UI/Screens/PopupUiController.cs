using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Screens
{
    public class PopupUiController : ScreenBase
    {
        public TMP_Text messageText;
        public TMP_Text headerText;
        public Button okButton;
        
        public void Show(string header, string msg, UnityAction onClose = null)
        {
            headerText.text = header;
            messageText.text = msg;
            okButton.onClick.AddListener(() => onClose?.Invoke());
            okButton.onClick.AddListener(OnClose);
        }

        private void OnClose()
        {
            okButton.onClick.RemoveAllListeners();
            Close();
        }
    }
}