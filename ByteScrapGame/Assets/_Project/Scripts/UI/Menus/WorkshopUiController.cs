using System;
using System.Collections.Generic;
using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.LevelContexts;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class WorkshopItem
    {
        public string id;
        public string author_id;
        public string author_name;
        public string name;
        public string description;
        public string image;
        public string uploaded_at;
    }
    
    public class WorkshopUiController : ScreenBase
    {
        [SerializeField] private GameObject schemeItemPrefab;
        [SerializeField] private Button loadSchemeButton;
        [SerializeField] private Button uploadSchemeButton;
        [SerializeField] private GameObject schemeItemContainer;
        
        [SerializeField] private TMP_Text selSchemeNameText;

        private string _selectedItemId;
        private Dictionary<string, SchemeItemController> _schemeItems = new();
        private bool _downloadLock;

        public override void Init()
        {
            base.Init();
            loadSchemeButton.onClick.AddListener(OnLoadSchemeClicked);
            uploadSchemeButton.onClick.AddListener(Bootstrap.Instance.ui.upload.Open);
        }
        

        public void UpdateItems(List<WorkshopItem> items)
        {
            Clear();
            foreach (WorkshopItem item in items)
            {
                var itemObj = Instantiate(schemeItemPrefab, schemeItemContainer.transform).GetComponent<SchemeItemController>();
                _schemeItems.Add(item.id, itemObj);
                itemObj.Init(item.id, item.description, () => OnItemClicked(item));
            }
        }
        public void Clear()
        {
            foreach (var item in _schemeItems)
            {
                Destroy(item.Value.gameObject);
            }
            _schemeItems.Clear();
        }

        private void OnItemClicked(WorkshopItem item)
        {
            _selectedItemId = item.id;
            Debug.Log($"Selected item {item.name} ({_selectedItemId})");
            selSchemeNameText.text = $"Выбранная схема: {item.name}";
        }

        private void OnLoadSchemeClicked()
        {
            if (_downloadLock || _selectedItemId == null) return;
            Debug.Log($"Loading scheme (ID: {_selectedItemId})");
            _downloadLock = true;
            Bootstrap.Instance.api.DownloadWorkshopItem(_selectedItemId, (code, json) =>
            {
                var data = JsonConvert.DeserializeObject<List<GridCellData>>(json);
                var context = FindAnyObjectByType<BuildingLevelContext>();
                context.buildingSystem.ClearGrid();
                context.buildingSystem.LoadGrid(data);
                _downloadLock = false;
                _selectedItemId = null;
                selSchemeNameText.text = "";
                Close();
            });
        }


        public override void Open()
        {
            base.Open();
            Bootstrap.Instance.api.GetWorkshopItems((code, data) =>
            {
                var items = JsonConvert.DeserializeObject<List<WorkshopItem>>(data);
                UpdateItems(items);
            });
        }

        private void Update()
        {
            loadSchemeButton.interactable = !(_downloadLock || _selectedItemId == null);
        }
    }
}