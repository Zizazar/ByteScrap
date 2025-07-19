using System.IO;
using UnityEngine;

namespace _Project.Scripts.ElectricitySystem
{
    public class CircuitSaver
    {
        public void SaveCircuit(string filename)
        {
            var data = CircuitManager.Instance.GetSaveData();
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, filename), json);
        }

        public void LoadCircuit(string filename)
        {
            string path = Path.Combine(Application.persistentDataPath, filename);
            if (!File.Exists(path)) return;
            
            string json = File.ReadAllText(path);
            CircuitData data = JsonUtility.FromJson<CircuitData>(json);
            CircuitManager.Instance.LoadFromData(data);
        }
    }
}