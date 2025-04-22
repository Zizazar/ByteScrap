namespace _Project.Scripts.GameRoot.States.PlayerStates
{
    public class BuildingPState : IState
    {
        public void Enter()
        {
            Bootstrap.Instance.input.Building.Enable();
            
        }

        public void Exit()
        {
            Bootstrap.Instance.input.Building.Disable();

        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }
    }
}