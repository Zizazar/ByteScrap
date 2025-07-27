using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.GameRoot.LevelContexts;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class ComponentSelectUiController : ScreenBase
    {
        [SerializeField] private GameObject buttonsArea;
        [SerializeField] private GameObject buttonPrefab;
        
        private List<SelectButtonController> _buttons = new ();
        private int _last_button_selection = -1;
        
        
        private CircuitManager _circuitManager;
        private BuildingSystem _buildingSystem;
        
        private void Start()
        {
            StartCoroutine(CO_Start());
            var sceneContext = FindAnyObjectByType<BuildingLevelContext>();

            _buildingSystem = sceneContext.buildingSystem;
        }

        private IEnumerator CO_Start()
        {
            
            yield return new WaitUntil(() => ComponentsRenderer.Instance.IsRendered);
            
            var renders = ComponentsRenderer.Instance.GetAllRenders();
            Debug.Log(renders.Count);
            for (int i = 0; i < renders.Count; i++)
            {
                var newButton = 
                    Instantiate(buttonPrefab, buttonsArea.transform, false)
                        .GetComponent<SelectButtonController>();
                
                newButton.Init(renders[i]);
                _buttons.Insert(i, newButton);
                newButton.index = i;
                newButton.button.onClick.AddListener(() => SelectComponent(newButton.index));
            }
        }

        private void SelectComponent(int index)
        {
            if (index == _last_button_selection) return;
            if (_last_button_selection != -1) _buttons[_last_button_selection].Select(false);
            var button = _buttons[index];
            button.Select(true);
            _last_button_selection = index;
            _buildingSystem.SelectComponent(index);
        }
    }
}