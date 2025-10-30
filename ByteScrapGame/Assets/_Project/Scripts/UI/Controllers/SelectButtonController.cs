using System;
using _Project.Scripts.GameRoot;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class SelectButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            Bootstrap.Instance.ui.hint.Show(componentTypeName);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Bootstrap.Instance.ui.hint.Hide();
        }
    }
}