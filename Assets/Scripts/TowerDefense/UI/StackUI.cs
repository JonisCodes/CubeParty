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
        private Canvas _canvas;

        private StatusInstance _instance;

        private void Update()
        {
            transform.LookAt(transform.position + _canvas.worldCamera.transform.rotation * Vector3.forward,
                _canvas.worldCamera.transform.rotation * Vector3.up);
        }

        private void OnDestroy()
        {
            if (_instance is not null)
                _instance.OnStacksChanged -= UpdateDisplay;
        }

        public void Init(StatusInstance instance, Canvas inCanvas)
        {
            _instance = instance;
            _instance.OnStacksChanged += UpdateDisplay;
            _canvas = inCanvas;
            UpdateDisplay(instance.Stacks);
            _canvas.worldCamera = Camera.main;
            background.color = _instance.Definition.BackgroundColor;
        }

        private void UpdateDisplay(int stacks)
        {
            label.text = stacks.ToString();
        }
    }
}