using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.ElectricitySystem.Systems.Responses;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.LevelContexts;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Screens
{
    public class UploadUiController : ScreenBase
    {
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private TMP_InputField descriptionField;
        [SerializeField] private Button submitButton;

        public override void Init()
        {
            base.Init();
            submitButton.onClick.AddListener(OnSubmitClicked);
        }

        private void OnSubmitClicked()
        {
            var context = FindAnyObjectByType<BuildingLevelContext>();
            var data = context.saveSystem.SaveGrid();
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var work = new WorkUpload(nameField.text, descriptionField.text, json);
            Bootstrap.Instance.api.UploadWork(work, (code, msg) =>
            {
                Close();
            });
        }
    }
}