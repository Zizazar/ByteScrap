using System.Collections;
using _Project.Scripts.GameRoot.LevelContexts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class LoadingLevelState : IState
    
    {
        public void Enter()
        {
            Bootstrap.Instance.StartCoroutine(CO_LoadScene(Bootstrap.Instance.LevelToLoad));
        }

        public void Exit()
        {
            Bootstrap.Instance.LevelToLoad = null;
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }
        
        private IEnumerator CO_LoadScene(string sceneName)
        {
            Bootstrap.Instance.ui.loadingScreen.FadeIn();
        
            // Начало асинхронной загрузки уровня
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            if (asyncOperation == null) yield break;
            
            yield return new WaitUntil(() => asyncOperation.isDone);

            var levelContext = Object.FindAnyObjectByType<LevelContext>();
            levelContext!.InitLevel();
            
            Bootstrap.Instance.ui.loadingScreen.FadeOut();
        }
    }
}