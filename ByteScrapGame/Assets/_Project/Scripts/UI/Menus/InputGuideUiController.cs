using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class InputGuideUiController : ScreenBase
    {
        [SerializeField] private Button exitButton;

        public override void Init()
        {
            base.Init();
            exitButton.onClick.AddListener(Close);
        }
    }
}