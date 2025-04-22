using System;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public abstract class ScreenBase : MonoBehaviour
    {
        private GameObject _root;

        public void Init()
        {
            _root = GetComponentInChildren<Canvas>().gameObject;
            _root.SetActive(false); // выключаем по умолчанию
        }

        public void Open() => _root.SetActive(true);
        public void Close() => _root.SetActive(false);
        public bool isOpened => _root.activeSelf;
    }
}