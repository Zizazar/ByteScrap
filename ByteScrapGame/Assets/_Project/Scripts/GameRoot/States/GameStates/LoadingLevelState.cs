using System.Collections;
using _Project.Scripts.GameRoot.LevelContexts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class LoadingLevelState : IState
    
    {
        
        private LevelData _levelData;
        
        
        public LoadingLevelState(LevelData levelData)
        {
            _levelData = levelData;
        }

        public void Enter()
        {
            Bootstrap.Instance.ui.loadingScreen.FadeIn();
            Bootstrap.Instance.StartCoroutine(CO_LoadScene());
        }

        public void Exit()
        {
            Bootstrap.Instance.ui.loadingScreen.FadeOut();
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }
        
        private IEnumerator CO_LoadScene()
        {
            // Начало асинхронной загрузки уровня
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
            if (asyncOperation == null) yield break;
            
            yield return new WaitUntil(() => asyncOperation.isDone);
            
            yield return ComponentsRenderer.Instance.Render();
            
            var levelContext = Object.FindAnyObjectByType<LevelContext>();
            levelContext!.InitLevel(_levelData);
        }
    }
}