using System.Collections;
using _Project.Scripts.UI;
using UnityEngine;

namespace _Project.Scripts.GameRoot.LevelContexts
{
    public class LevelContext : MonoBehaviour
    {
        public virtual IEnumerator InitLevel(string id, SaveTypes saveType)
        {
            return null;
        }
        public virtual IEnumerator DisposeLevel()
        {
            return null;
        }
    }
}