using _Project.Scripts.GameRoot.LevelContexts;
using _Project.Scripts.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _Project.Scripts.GameRoot.States.PlayerStates
{
    public class BuildingPState : IState
    {
        private CircuitManager _circuitManager;
        private BuildingSystem _buildingSystem;
        
        public void Enter()
        {
            var sceneContext = Object.FindAnyObjectByType<BuildingLevelContext>();

            _buildingSystem = sceneContext.buildingSystem;
            
            Bootstrap.Instance.input.Building.Enable();
            //string json = Resources.Load<TextAsset>("Levels/lvl_1").text;
            //Debug.Log(JsonUtility.FromJson<LevelJsonData>(json).name);
            
            Bootstrap.Instance.input.Building.Place.performed += PlaceComponentAction;
            Bootstrap.Instance.input.Building.Remove.performed += RemoveComponentAction;

            Bootstrap.Instance.ui.componentSelect.Open();
        }

        public void Exit()
        {
            Bootstrap.Instance.input.Building.Disable();
            Bootstrap.Instance.input.Building.Place.performed -= PlaceComponentAction;
            Bootstrap.Instance.input.Building.Remove.performed -= RemoveComponentAction;
            
            Bootstrap.Instance.ui.componentSelect.Close();
        }

        public void Update()
        {
            _buildingSystem?.UpdateHologram();
        }

        public void FixedUpdate()
        {
        }

        private void RemoveComponentAction(InputAction.CallbackContext ctx)
        {
            if (Bootstrap.Instance.ui.componentSelect.IsMouseOver()) return;
            _buildingSystem.TryRemoveComponent();
        }

        private void PlaceComponentAction(InputAction.CallbackContext ctx)
        {
            if (Bootstrap.Instance.ui.componentSelect.IsMouseOver()) return;
            _buildingSystem.TryPlaceComponent();
        }
        
    }
}