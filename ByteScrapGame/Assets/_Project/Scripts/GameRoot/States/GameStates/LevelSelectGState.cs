namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class LevelSelectGState : IState
    {
        public void Enter()
        {
            Bootstrap.Instance.ui.levelSelect.Open();
        }

        public void Exit()
        {
            Bootstrap.Instance.ui.levelSelect.Close();

        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }
    }
}