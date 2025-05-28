namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class BuildingGState : IState
    {
        public void Enter()
        {
            Bootstrap.Instance.LoadScene("SampleScene");
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    
        public void FixedUpdate()
        {
        }
    }
}