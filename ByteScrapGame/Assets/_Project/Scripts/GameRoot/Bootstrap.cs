using System;
using System.Collections;
using _Project.Scripts.Commands;
using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.GameRoot.StateMacines;
using _Project.Scripts.GameRoot.States.GameStates;
using _Project.Scripts.GameRoot.States.PlayerStates;
using _Project.Scripts.Player;
using _Project.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Project.Scripts.GameRoot
{
    public class Bootstrap : MonoBehaviour
    {
        
        private static Bootstrap _instance;

        public static Bootstrap Instance // Singleton
        {
            get
            {
                // Ленивая инициализация при первом обращении
                if (_instance == null)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    _instance = FindObjectOfType<Bootstrap>();
#pragma warning restore CS0618 // Type or member is obsolete
                
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("BOOTSTRAP");
                        _instance = singletonObject.AddComponent<Bootstrap>();
                    }
                }
                return _instance;
            }
        }
        
        
        [SerializeField] private GameSettings settingsObject;

        // -----Референсы классов для подтягивания зависимостей--------
        public GameStateMachine sm_Game { get; private set; }
        public PlayerStateMachine sm_Player { get; private set; }
        public GameSettings settings => settingsObject;
        public PlayerController playerController { get; private set; }
        public GameInput input { get; private set; }
        public CircuitManager circuitManager { get; private set; }
        public DebugCommands debugCmd { get; private set; }
        public UiController ui { get; private set; }
        // ------------------------------------------------------------
        private void Start()
        {
            // Один эксемпляр на сцене (Singleton)
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            
            DontDestroyOnLoad(this.gameObject);
            
            // ---Иницилизация основной логики---
            
            ui = GetComponentInChildren<UiController>(); // UI должен быть в BOOTSTRAP
            ui.Init();
            
            sm_Game = GetComponent<GameStateMachine>();
            sm_Game.ChangeState(new MenuGState());
            
            
            
            input = new GameInput();
            input.Enable();

            debugCmd = new DebugCommands();
            debugCmd.Init();
            
            

        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void InitLevel() 
        {
            
            playerController = FindAnyObjectByType<PlayerController>();
            playerController.Init();
            
            circuitManager = new CircuitManager();
            circuitManager.Init();
            
            
            
        }


        PlayerController SpawnPlayer()
        {
            GameObject spawnedPlayer = Instantiate(settings.player.playerPrefab, settings.player.spawnPos, settings.player.spawnRot);
            return spawnedPlayer?.GetComponent<PlayerController>();
        }

        public void SpawnCube(Vector3 position)
        {
            // Тестовый метод для спавна куба по команде
            var spawnedObject = GameObject.CreatePrimitive( PrimitiveType.Cube );
            spawnedObject.transform.position = position;
            spawnedObject.GetComponent<Renderer>().material.color = Color.HSVToRGB(Random.value, 1f, 1f);
            spawnedObject.AddComponent<BoxCollider>();
            spawnedObject.AddComponent<Rigidbody>();
        }
        
        // Загрузка уровня
        public void LoadScene(string sceneName)
        {
            StartCoroutine(CO_LoadScene(sceneName));
        }
        
        private IEnumerator CO_LoadScene(string sceneName)
        {
            ui.loadingScreen.FadeIn();
        
            // Начало асинхронной загрузки уровня
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            if (asyncOperation == null) yield break;
            
            yield return new WaitUntil(() => asyncOperation.isDone);

            InitLevel();
            
            ui.loadingScreen.FadeOut();
        }
    }
}
