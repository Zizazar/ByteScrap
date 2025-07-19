using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.GameRoot.States.PlayerStates
{
    public class BuildingPState : IState
    {
        PlayerController _player;
        
        public void Enter()
        {
            _player = Bootstrap.Instance.playerController;
            
            Bootstrap.Instance.input.Building.Enable();
            string json = Resources.Load<TextAsset>("Levels/lvl_1").text;
            Debug.Log(JsonUtility.FromJson<LevelJsonData>(json).name);
            
            _player.interactionController.EnableInteraction();
        }

        public void Exit()
        {
            Bootstrap.Instance.input.Building.Disable();
            _player.interactionController.DisableInteraction();
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }
        
        
    }
}