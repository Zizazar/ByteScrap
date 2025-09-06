using _Project.Scripts.GameRoot.StateMacines;
using _Project.Scripts.GameRoot.States.PlayerStates;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class MenuGState : IState
    {

        public void Enter()
        {
            SceneManager.LoadScene("Menu");
            Bootstrap.Instance.ui.menu.Open();
            Bootstrap.Instance.sm_Player?.ChangeState(new MenuViewPState());
            Bootstrap.Instance.sm_Game.ChangeState(new LevelSelectGState());
        }

        public void Exit()
        {
            Bootstrap.Instance.ui.menu.Close();

        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }
    }
}