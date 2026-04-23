using TMPro;
using TowerDefense.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.UI
{
    public class StackUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        [SerializeField] private Image background;

        private StatusInstance _instance;

        private void OnDestroy()
        {
            if (_instance is not null)
                _instance.OnStacksChanged -= UpdateDisplay;
        }

        public void Init(StatusInstance instance)
        {
            _instance = instance;
            _instance.OnStacksChanged += UpdateDisplay;
            UpdateDisplay(instance.Stacks);
            background.color = _instance.Definition.backgroundColor;
        }

        private void UpdateDisplay(int stacks)
        {
            label.text = stacks.ToString();
        }
    }
}