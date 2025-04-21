using _Project.Scripts.Settings;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "GameSettings/PlayerSettings")]
[System.Serializable]
public class PlayerSettings : ScriptableObject, IGameSettings
{
    public GameObject playerPrefab;
    public Vector3 spawnPos;
    public Quaternion spawnRot;
    public float movementSpeed;
}
