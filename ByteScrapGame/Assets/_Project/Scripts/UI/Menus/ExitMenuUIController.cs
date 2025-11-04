using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.LevelContexts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ExitMenuUIController : ScreenBase
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button settingsButton;

        public override void Init()
        {
            exitButton.onClick.AddListener(ExitAction);
            closeButton.onClick.AddListener(Close);
            settingsButton.onClick.AddListener(OpenSettings);
            base.Init();
        }

        private void OpenSettings()
        {
            Bootstrap.Instance.ui.settings.Open();
            Close();
        }

        private void ExitAction()
        {
            var levelContext = FindAnyObjectByType<BuildingLevelContext>();
            StartCoroutine(levelContext?.DisposeLevel());
            Close();
        }
        
    }
}