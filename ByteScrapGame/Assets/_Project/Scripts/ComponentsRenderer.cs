using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class ComponentsRenderer : MonoBehaviour
    {
        [SerializeField] private Camera renderCamera;
        [SerializeField] private Transform pivotPoint;

        [SerializeField] private int width;
        [SerializeField] private int height;
        
        [SerializeField] private RawImage image;
        [SerializeField] private Button button;
        [SerializeField] private Button button2;
        [SerializeField] private TMP_Text label;

        #region Singleton
        private static ComponentsRenderer _instance;
        public static ComponentsRenderer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<ComponentsRenderer>();
                
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("ComponentsRenderer");
                        _instance = singletonObject.AddComponent<ComponentsRenderer>();
                    }
                }
                return _instance;
            }
        }
        

        #endregion
        
        private Dictionary<string, RenderTexture> _cachedRenders = new ();

        private int _currentImageIndex;
        private bool _isRendered;
        
        public void Render()
        {
            _currentImageIndex = 0;
            _cachedRenders.Clear();
            StartCoroutine(CO_Render());

        }

        private IEnumerator CO_Render()
        {
            foreach (var prefab in Resources.LoadAll<GameObject>("Components"))
            {
                var objectToRender = Instantiate(prefab, pivotPoint);
                
                foreach (var trans in objectToRender.GetComponentsInChildren<Transform>())
                    trans.gameObject.layer = LayerMask.NameToLayer("Render");
                
                label.text = objectToRender.name;
                
                var renderTexture = new RenderTexture(width, height, 24);
                renderCamera.targetTexture = renderTexture;
                renderCamera.Render();
                yield return new WaitForEndOfFrame();
                renderCamera.targetTexture = null;
                _cachedRenders.Add(objectToRender.name, renderTexture);
                Destroy(objectToRender);
                yield return new WaitUntil(() => objectToRender.IsDestroyed());
            }
            _isRendered = true;
        }

        private void Start()
        {
            Render();
            button.onClick.AddListener(SetImage);
            button2.onClick.AddListener(Render);
        }

        private void SetImage()
        {
            if (_cachedRenders.Count == 0) return;
            image.texture = GetAllRenders()[_currentImageIndex];
            _currentImageIndex++;
            if (_currentImageIndex >= _cachedRenders.Count) _currentImageIndex = 0;
        }
        
        public RenderTexture GetRender(string componentName)
        {
            if (!_isRendered) Render();
            return _cachedRenders.GetValueOrDefault(componentName);
        }

        public List<RenderTexture> GetAllRenders()
        {
            if (!_isRendered) Render();
            return _cachedRenders.Values.ToList();
        }
        
        public bool IsRendered => _isRendered;
    }
}