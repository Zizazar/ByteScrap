using System;
using TMPro;
using UnityEngine;

namespace _Project.Scripts
{
    public class HintSystem : MonoBehaviour
    {
        
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform root;
        [SerializeField] private TMP_Text headerText;
        public void Show(string header)
        {
            headerText.text = header;
            canvas.gameObject.SetActive(true);
        }

        public void Hide()
        {
            canvas.gameObject.SetActive(false);
        }

        private void Update()
        {
            root.position = 
                Vector3.Lerp(root.position, Input.mousePosition, 0.1f);
        }
        
    }
}