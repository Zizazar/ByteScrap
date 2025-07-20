namespace _Project.Scripts.GameRoot.States.PlayerStates
{
    public class InteractPState : IState
    {
        public void Enter()
        {
            Bootstrap.Instance.playerController.interactionController.EnableInteraction();
            Bootstrap.Instance.input.Interaction.Enable();
        }

        public void Exit()
        {
            Bootstrap.Instance.playerController.interactionController.DisableInteraction();
            Bootstrap.Instance.input.Interaction.Disable();
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }
    }
}