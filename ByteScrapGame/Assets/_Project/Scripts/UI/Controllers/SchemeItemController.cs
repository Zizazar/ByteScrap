using _Project.Scripts.GameRoot;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class SchemeItemController : MonoBehaviour
    {
        [SerializeField] private TMP_Text headerText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button button;

        public void Init(string name, string description, UnityAction onClick)
        {
            headerText.text = name;
            descriptionText.text = description;
            button.onClick.AddListener(onClick);
            
        }
    }
}