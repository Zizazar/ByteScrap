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
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void FixedUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}