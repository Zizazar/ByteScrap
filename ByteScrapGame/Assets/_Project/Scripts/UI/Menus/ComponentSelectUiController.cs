using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GameRoot.LevelContexts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.UI
{
    public class ComponentSelectUiController : ScreenBase
    {
        [SerializeField] private GameObject buttonsArea;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private MouseHoverHelper rootMouseHover;
        
        [SerializeField] private RectTransform panel;
        
        private Dictionary<string, SelectButtonController> _buttons = new ();
        private string _lastButtonSelection;
        private Tweener _tweener;
        
        private CircuitManager _circuitManager;
        private BuildingSystem _buildingSystem;
        
        private bool _initialized;

        public void Initialize()
        {
            var sceneContext = FindAnyObjectByType<BuildingLevelContext>();
            _buildingSystem = sceneContext.buildingSystem;
            ClearButtons();
            StartCoroutine(CO_Start());
            
        }

        private void ClearButtons()
        {
            foreach (var button in _buttons)
            {
                Destroy(button.Value.gameObject);
            }
            _buttons.Clear();
        }
        
        private IEnumerator CO_Start()
        {
            
            yield return new WaitUntil(() => ComponentsRenderer.Instance.IsRendered);
            
            var renders = ComponentsRenderer.Instance.GetAllRendersMapped();
            foreach(KeyValuePair<string, RenderTexture> render in renders)
            {
                var newButton = 
                    Instantiate(buttonPrefab, buttonsArea.transform, false)
                        .GetComponent<SelectButtonController>();
                
                newButton.Init(render.Key, render.Value);
                _buttons.Add(render.Key, newButton);
                newButton.button.onClick.AddListener(() => SelectComponent(newButton.componentTypeName));
            }
        }

        private void SelectComponent(string componentTypeName)
        {
            if (componentTypeName == _lastButtonSelection) return;
            if (_lastButtonSelection != null) _buttons[_lastButtonSelection].Select(false);
            var button = _buttons[componentTypeName];
            button.Select(true);
            _lastButtonSelection = componentTypeName;
            _buildingSystem.SelectComponent(componentTypeName);
        }

        public bool IsMouseOver()
        {
            return rootMouseHover.MouseOver;
        }

        public override void Open()
        {
            base.Open();
            _tweener?.Kill();
            _tweener = panel.DOAnchorPosX(75, 0.3f);
        }

        public override void Close()
        {
            _tweener?.Kill();
            _tweener = panel.DOAnchorPosX(-75, 0.3f)
                .OnComplete(base.Close);
            
        }
    }
}