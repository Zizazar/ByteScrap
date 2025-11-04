using System.Collections.Generic;
using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.LevelContexts;
using Newtonsoft.Json;
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
        [SerializeField] private GameObject schemeItemContainer;

        private string _selectedItemId;
        private List<SchemeItemController> _schemeItems = new();

        public void UpdateItems(List<WorkshopItem> items)
        {
            Clear();
            foreach (WorkshopItem item in items)
            {
                var itemObj = Instantiate(schemeItemPrefab, schemeItemContainer.transform).GetComponent<SchemeItemController>();
                _schemeItems.Add(itemObj);
                itemObj.Init(item.id, item.description, () => OnItemClicked(item));
            }
        }
        public void Clear()
        {
            foreach (var item in _schemeItems)
            {
                Destroy(item.gameObject);
            }
            _schemeItems.Clear();
        }

        private void OnItemClicked(WorkshopItem item)
        {
            _selectedItemId = item.id;
        }

        private void OnLoadSchemeClicked()
        {
            Bootstrap.Instance.api.DownloadWorkshopItem(_selectedItemId, (code, json) =>
            {
                var data = JsonConvert.DeserializeObject<List<GridCellData>>(json);
                var context = FindAnyObjectByType<BuildingLevelContext>();
                context.buildingSystem.LoadGrid(data);
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
    }
}