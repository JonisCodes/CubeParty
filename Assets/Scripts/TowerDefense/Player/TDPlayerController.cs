using TowerDefense.Towers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TowerDefense.Player
{
    public class TdPlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float turnSpeed;

        [SerializeField] private float clickRange;
        [SerializeField] private LayerMask interactableLayer;

        private Tower _selectedTower;

        private Vector2 _wasdInput;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        private void Update()
        {
            var mousePosition = Mouse.current.position.ReadValue();

            var edgeInput = Vector2.zero;
            if (mousePosition.x > Screen.width - 100) edgeInput.x = 1;
            else if (mousePosition.x < 100) edgeInput.x = -1;
            if (mousePosition.y > Screen.height - 100) edgeInput.y = 1;
            else if (mousePosition.y < 100) edgeInput.y = -1;

            var move = (_wasdInput + edgeInput).normalized;
            transform.position += new Vector3(move.x * moveSpeed, 0, move.y * moveSpeed) * Time.deltaTime;
        }

        private void OnMove(InputValue value)
        {
            _wasdInput = value.Get<Vector2>();
        }

        private void OnClick(InputValue value)
        {
            var cam = Camera.main;
            if (!value.isPressed || cam is null) return;

            var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out var hit, clickRange, interactableLayer)) return;

            _selectedTower?.Uninteract();

            _selectedTower = hit.collider.GetComponent<Tower>();
            _selectedTower?.Interact();
        }
    }
}