using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class SelectButtonController : MonoBehaviour
    {
        public Button button;
        public RawImage rawImage;
        public int index;

        [SerializeField] private GameObject selectVisual;
        public void Init(RenderTexture renderTexture)
        {
            button = GetComponent<Button>();
            rawImage = GetComponent<RawImage>();
            
            rawImage.texture = renderTexture;
        }

        public void Select(bool state)
        {
            selectVisual.SetActive(state);
        }
    }
}