using _Project.Scripts.UI.Screens;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class UiController : MonoBehaviour
    {
        public MenuUiController menu;
        public LevelSelectUiController levelSelect;
        public LoadingScreenController loadingScreen;
        public ComponentSelectUiController componentSelect;
        public InGameOverlayController inGameOverlay;
        public SettingsUiController settings;
        public AuthUiController auth;
        public PopupUiController popup;
        public HintSystem hint;
        public ExitMenuUIController exitMenu;
        public InputGuideUiController inputGuide;
        public WorkshopUiController workshop;
        public UploadUiController upload;
        
        
        public void Init()
        {
            menu.Init();
            levelSelect.Init();
            loadingScreen.Init();
            componentSelect.Init();
            inGameOverlay.Init();
            settings.Init();
            auth.Init();
            popup.Init();
            exitMenu.Init();
            inputGuide.Init();
            workshop.Init();
            upload.Init();
        }
    }
}