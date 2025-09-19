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
        
        
        public void Init()
        {
            menu.Init();
            levelSelect.Init();
            loadingScreen.Init();
            componentSelect.Init();
            inGameOverlay.Init();
        }
    }
}