using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ComponentSelectUiController : ScreenBase
    {
        [SerializeField] private GameObject buttonsArea;
        [SerializeField] private GameObject buttonPrefab;
        private void Start()
        {
            StartCoroutine(CO_Start());
        }

        private IEnumerator CO_Start()
        {
            
            yield return new WaitUntil(() => ComponentsRenderer.Instance.IsRendered);
            
            var renders = ComponentsRenderer.Instance.GetAllRenders();
            Debug.Log(renders.Count);
            foreach (var render in renders)
            {
                var newButton = Instantiate(buttonPrefab, buttonsArea.transform, false);
                var rawImage = newButton.GetComponent<RawImage>();
                rawImage.texture = render;
            }
        }
    }
}