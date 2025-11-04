using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public abstract class ScreenBase : MonoBehaviour
    {
        private GameObject _root;
        private CanvasGroup _canvas;

        public virtual void Init()
        {
            Debug.Log(GetType().Name);
            _canvas = GetComponentInChildren<CanvasGroup>();
            _root = _canvas.gameObject;
            _root.SetActive(false); // выключаем по умолчанию
        }

        public virtual void Open() => _root.SetActive(true);
        public virtual void Close() => _root.SetActive(false);
        public bool isOpened => _root.activeSelf;
        
        public void Toggle() => _root.SetActive(!_root.activeSelf);
        
        public Tweener FadeIn()
        {
            _canvas.alpha = 0;
            Open();
            return _canvas.DOFade(1, 1f);
        }
        public Tweener FadeOut()
        {
           return _canvas.DOFade(0, 1f).OnComplete(Close);
        }

    }
}