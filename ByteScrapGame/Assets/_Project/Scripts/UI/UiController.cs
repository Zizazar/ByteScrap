using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class UiController : MonoBehaviour
    {
        public MenuUiController menu;
        public LevelSelectUiController levelSelect;

        public void Init()
        {
            menu.Init();
            levelSelect.Init();
        }
    }
}