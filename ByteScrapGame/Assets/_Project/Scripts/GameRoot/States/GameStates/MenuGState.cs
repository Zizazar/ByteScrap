using _Project.Scripts.GameRoot.StateMacines;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class MenuGState : IState
    {

        public void Enter()
        {
            SceneManager.LoadScene("Menu");
            Bootstrap.Instance.ui.menu.Open();
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