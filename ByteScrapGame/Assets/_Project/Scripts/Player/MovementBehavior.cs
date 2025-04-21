using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
    public class MovementBehavior : IPlayerBehavior
    {
        private readonly GameInput _input;
        private readonly PlayerController _player;
        private readonly GameSettings _settings;

        public MovementBehavior(PlayerController player, GameInput input, GameSettings settings)
        {
            _player = player;
            _input = input;
            _settings = settings;
        }

        public void Enter()
        {
            _input.Player.Enable();
            _input.Player.Move.performed += OnMovePerformed;
        }

        public void Exit()
        {
            _input.Player.Disable();

        }

        public void Update() { }

        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            Vector2 moveInput = _input.Player.Move.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        
            _player.Move(moveDirection * _settings.player.movementSpeed * Time.deltaTime);
        }
    }
}