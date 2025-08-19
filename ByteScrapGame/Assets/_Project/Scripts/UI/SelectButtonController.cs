using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class SelectButtonController : MonoBehaviour
    {
        public Button button;
        public RawImage rawImage;
        public string componentTypeName;

        [SerializeField] private GameObject selectVisual;
        public void Init(string typeName,RenderTexture renderTexture)
        {
            button = GetComponent<Button>();
            rawImage = GetComponent<RawImage>();
            
            rawImage.texture = renderTexture;
            componentTypeName = typeName;
        }

        public void Select(bool state)
        {
            selectVisual.SetActive(state);
        }
    }
}