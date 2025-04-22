using _Project.Scripts.GameRoot.StateMacines;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class MenuState : IState
    {

        public void Enter()
        {
            SceneManager.LoadScene("Menu");
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