using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public abstract class ScreenBase : MonoBehaviour
    {
        private GameObject _root;
        private CanvasGroup _canvas;

        public void Init()
        {
            Debug.Log(GetType().Name);
            _canvas = GetComponentInChildren<CanvasGroup>();
            _root = _canvas.gameObject;
            _root.SetActive(false); // выключаем по умолчанию
        }

        public virtual void Open() => _root.SetActive(true);
        public virtual void Close() => _root.SetActive(false);
        public bool isOpened => _root.activeSelf;
        
        public void FadeIn()
        {
            _canvas.alpha = 0;
            Open();
            _canvas.DOFade(1, 1f);
        }
        public void FadeOut()
        {
            _canvas.DOFade(0, 1f).OnComplete(Close);
        }

    }
}