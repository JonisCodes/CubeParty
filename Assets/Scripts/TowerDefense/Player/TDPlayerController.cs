using TowerDefense.Managers;
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
        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private PlacementGrid grid;
        [SerializeField] private GameObject ghostTowerPrefab;
        private Transform _ghostTransform;
        private Camera _mainCamera;
        private bool _placingTower;

        private Tower _selectedTower;

        private Vector2 _wasdInput;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            _mainCamera = Camera.main;
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
            var moveDir = transform.right * move.x + transform.forward * move.y;
            transform.position += moveDir * (moveSpeed * Time.deltaTime);


            if (!_placingTower) return;

            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, groundLayer)) return;

            var cellPos = grid.WorldToCell(hit.point);
            _ghostTransform.position = grid.CellToWorldSurface(cellPos);

            if (grid.IsValid(cellPos))
            {
            }
        }

        private void OnMove(InputValue value)
        {
            _wasdInput = value.Get<Vector2>();
        }

        private void OnClick(InputValue value)
        {
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (_placingTower)
            {
                if (!Physics.Raycast(ray, out var groundHit, Mathf.Infinity, groundLayer)) return;

                var cellPos = grid.WorldToCell(groundHit.point);
                var ghostTower = _ghostTransform.GetComponent<GhostTower>();
                if (TowerManager.Instance.PlaceTower(ghostTower.towerPrefab, cellPos))
                {
                    _placingTower = false;
                    Destroy(_ghostTransform.gameObject);
                    _ghostTransform = null;
                }
            }

            if (!value.isPressed || _mainCamera is null) return;

            if (!Physics.Raycast(ray, out var hit, clickRange, interactableLayer)) return;

            // Reset prev tower before new tower is interacted with
            _selectedTower?.Uninteract();

            _selectedTower = hit.collider.GetComponent<Tower>();
            _selectedTower?.Interact();
        }

        private void OnTempBuy()
        {
            _selectedTower = null;
            _ghostTransform = null;

            if (ghostTowerPrefab is null) return;

            var ghost = Instantiate(ghostTowerPrefab);
            _ghostTransform = ghost.transform;
            _placingTower = true;
        }
    }
}