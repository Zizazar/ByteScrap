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
            
        }

        public void Exit()
        {
            Bootstrap.Instance.input.Building.Disable();

        }

        public void Update()
        {
            _player.HandleInteraction();
        }

        public void FixedUpdate()
        {
        }
    }
}