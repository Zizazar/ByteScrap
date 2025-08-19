using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.UI
{
    public class MouseHoverHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool MouseOver = false;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            MouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MouseOver = false;
        }
    }
}