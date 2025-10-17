using System.Collections;
using _Project.Scripts.GameRoot.LevelContexts;
using _Project.Scripts.UI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.GameRoot.States.GameStates
{
    public class LoadingLevelState : IState
    
    {
        
        private string _levelID;
        private SaveTypes _saveType;

        public LoadingLevelState(string id, SaveTypes dataSaveType)
        {
            _levelID = id;
            _saveType = dataSaveType;
        }

        public void Enter()
        {
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
            
            yield return Bootstrap.Instance.ui.loadingScreen.FadeIn().WaitForCompletion();
            
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
            if (asyncOperation == null) yield break;
            
            yield return new WaitUntil(() => asyncOperation.isDone);
            
            yield return ComponentsRenderer.Instance.Render();
            
            var levelContext = Object.FindAnyObjectByType<LevelContext>();
            yield return Bootstrap.Instance.StartCoroutine(
                levelContext!.InitLevel(_levelID, _saveType)
            );
        }
    }
}