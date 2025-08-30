using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GameRoot.LevelContexts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.UI
{
    public class ComponentSelectUiController : ScreenBase
    {
        [SerializeField] private GameObject buttonsArea;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private MouseHoverHelper rootMouseHover;
        
        private Dictionary<string, SelectButtonController> _buttons = new ();
        private string _last_button_selection;
        
        
        private CircuitManager _circuitManager;
        private BuildingSystem _buildingSystem;

        public void Initialize()
        {
            StartCoroutine(CO_Start());
            var sceneContext = FindAnyObjectByType<BuildingLevelContext>();

            _buildingSystem = sceneContext.buildingSystem;
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
            if (componentTypeName == _last_button_selection) return;
            if (_last_button_selection != null) _buttons[_last_button_selection].Select(false);
            var button = _buttons[componentTypeName];
            button.Select(true);
            _last_button_selection = componentTypeName;
            _buildingSystem.SelectComponent(componentTypeName);
        }

        public bool IsMouseOver()
        {
            return rootMouseHover.MouseOver;
        }
        
    }
}