using _Project.Scripts.Commands;
using _Project.Scripts.ElectricitySystem;
using _Project.Scripts.Player;
using _Project.Scripts.Settings;
using UnityEngine;

namespace _Project.Scripts.GameRoot
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private GameSettings settingsObject;

        public GameSettings settings { get; private set; }
        
        public PlayerController playerController { get; private set; }
        public GameInput input { get; private set; }

        public CircuitManager circuitManager { get; private set; }
        public DebugCommands debugCmd { get; private set; }
        
        private void Awake()
        {
            settings = settingsObject;
            
            input = new GameInput();
            input.Enable();
            
            playerController = SpawnPlayer();
            
            playerController.Init(this);
            Debug.Log("Player init");
            
            circuitManager = new CircuitManager();
            circuitManager.Init();

            debugCmd = new DebugCommands();
            debugCmd.Init(this);
            
            

            DontDestroyOnLoad(this.gameObject);
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
            spawnedObject.GetComponent<Renderer>().material.color = Color.green;
            spawnedObject.AddComponent<BoxCollider>();
            spawnedObject.AddComponent<Rigidbody>();
        }
    }
}
