using System;
using System.IO;
using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.GameRoot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class GameSettings
    {
        public FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        public int width;
        public int height;
        public bool levelAutoExit;
        public bool cloudSaves;

        public void ToPlayerPrefs()
        {
            PlayerPrefs.SetString("FullScreenMode", fullScreenMode.ToString());
            PlayerPrefs.SetInt("ResolutionWidth", width);
            PlayerPrefs.SetInt("ResolutionHeight", height);
            PlayerPrefs.SetInt("LevelAutoExit", levelAutoExit ? 1 : 0);
            PlayerPrefs.SetInt("CloudSaves", cloudSaves ? 1 : 0);
        }

        public void FromPlayerPrefs()
        {
            fullScreenMode = Enum.Parse<FullScreenMode>(PlayerPrefs.GetString("FullScreenMode", FullScreenMode.FullScreenWindow.ToString()), true);
            width = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width);
            height = PlayerPrefs.GetInt("ResolutionHeight", Screen.currentResolution.height);
            levelAutoExit = PlayerPrefs.GetInt("LevelAutoExit", 0) == 1;    
            cloudSaves = PlayerPrefs.GetInt("CloudSaves", 1) == 1;
        }

        public void Apply()
        {
            Screen.SetResolution(width, height, fullScreenMode);
        }
    }
    
    public class SettingsUiController : ScreenBase
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle autoExitToggle;

        [SerializeField] private Button clearSavesButton;
        
        private Resolution _selectedResolution;
        
        public override void Init()
        {
            base.Init();
            
            Resolution[] resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            foreach (var resolution in resolutions)
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
            resolutionDropdown.RefreshShownValue();
            resolutionDropdown.onValueChanged.AddListener((v) => _selectedResolution = resolutions[v]);
            
            var settings = Bootstrap.Instance.gameSettings;
            
            fullscreenToggle.isOn = settings.fullScreenMode == FullScreenMode.FullScreenWindow;
            autoExitToggle.isOn = settings.levelAutoExit;
            clearSavesButton.onClick.AddListener(ClearSaves);
        }

        private void ClearSaves()
        {
            SaveSystem.DeleteLocalSaves();
            
            
        }

        private void OnDestroy()
        {
            resolutionDropdown.onValueChanged.RemoveAllListeners();
        }


        public GameSettings Save()
        {
            var settings = Bootstrap.Instance.gameSettings;
            
            settings.height = _selectedResolution.height;
            settings.width = _selectedResolution.width;
            settings.fullScreenMode = fullscreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            settings.levelAutoExit = autoExitToggle.isOn;
            
            settings.ToPlayerPrefs();
            return settings;
        }

        public override void Close()
        {
            Save().Apply();
            base.Close();
        }
    }
}