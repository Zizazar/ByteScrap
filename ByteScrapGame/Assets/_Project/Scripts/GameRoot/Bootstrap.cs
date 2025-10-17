using _Project.Scripts.Commands;
using _Project.Scripts.ElectricitySystem.Systems;
using _Project.Scripts.GameRoot.LevelContexts;
using _Project.Scripts.GameRoot.StateMacines;
using _Project.Scripts.GameRoot.States.GameStates;
using _Project.Scripts.LevelAndGoals;
using _Project.Scripts.Player;
using _Project.Scripts.UI;
using UnityEngine;

using UnityEngine.UI;

namespace _Project.Scripts.GameRoot
{
    public class Bootstrap : MonoBehaviour
    {
        #region Singleton
        private static Bootstrap _instance;

        public static Bootstrap Instance
        {
            get
            {
                // Ленивая инициализация при первом обращении
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<Bootstrap>();
                
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("BOOTSTRAP");
                        _instance = singletonObject.AddComponent<Bootstrap>();
                    }
                }
                return _instance;
            }
        }
        #endregion
        
        [SerializeField]
        private GameObject playerPrefab;

        // -----Референсы классов для подтягивания зависимостей--------
        public GameStateMachine sm_Game { get; private set; }
        public PlayerController playerController { get; set; }
        public GameInput input { get; private set; }
        public DebugCommands debugCmd { get; private set; }
        public UiController ui { get; private set; }
        public GoalSystem goalSystem { get; private set; }
        
        public GameSettings gameSettings { get; private set; }
        
        public GameApi api { get; private set; }
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
            
            api = new GameApi();
            
            gameSettings = new GameSettings();
            gameSettings.FromPlayerPrefs();
            gameSettings.Apply();
            
            goalSystem = new GoalSystem();
            
            ui = GetComponentInChildren<UiController>(); // UI должен быть в BOOTSTRAP
            ui.Init();
            
            input = new GameInput();
            input.Enable();
            
            debugCmd = new DebugCommands();
            debugCmd.Init();
            
            
            sm_Game = GetComponent<GameStateMachine>();
            sm_Game.ChangeState(new MenuGState());
            
            //sm_Player = GetComponent<PlayerStateMachine>();
            //sm_Player.ChangeState(new MenuViewPState());

            
        }


        public PlayerController SpawnPlayer()
        {
            var foundPlayer = FindAnyObjectByType<PlayerController>();
            if (foundPlayer)
            {
                playerController = foundPlayer;
                foundPlayer.Init();
                return foundPlayer;
            }
            var spawnedPlayer = Instantiate(playerPrefab, transform
                ).GetComponent<PlayerController>();
            spawnedPlayer.Init();
            playerController = spawnedPlayer;
            return spawnedPlayer;
        }

        private void Update()
        {
            sm_Game?.Update();
        }

        private void FixedUpdate()
        {
            sm_Game?.FixedUpdate();
        }


        public void CloseLevel()
        {
            BuildingLevelContext buildingLevelContext = FindAnyObjectByType<BuildingLevelContext>();
            if (!buildingLevelContext) return; 
            StartCoroutine(buildingLevelContext.DisposeLevel());
        }
    }
}
