using System.Collections.Generic;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Managers
{
    public class PlacementGrid : MonoBehaviour
    {
        [SerializeField] private int maxWidth;
        [SerializeField] private int maxHeight;
        [SerializeField] private int cellSize;

        private Dictionary<Vector2Int, Tower> _cells; // if tower is null then cell is empty
        private Vector2 _gridOrigin;

        private void Awake()
        {
            CreateCells();
        }

        private void OnDrawGizmos()
        {
            var columns = Mathf.FloorToInt(maxWidth / cellSize);
            var rows = Mathf.FloorToInt(maxHeight / cellSize);
            var origin = new Vector2(transform.position.x, transform.position.z);

            Gizmos.color = Color.green;

            for (var x = 0; x < columns; x++)
            for (var y = 0; y < rows; y++)
            {
                var worldCenter = new Vector3(
                    origin.x + x * cellSize + cellSize * 0.5f,
                    transform.position.y,
                    origin.y + y * cellSize + cellSize * 0.5f
                );
                Gizmos.DrawWireCube(worldCenter, new Vector3(cellSize, 0.1f, cellSize));
            }
        }

        private void OnValidate()
        {
            CreateCells();
        }

        private void CreateCells()
        {
            transform.position = new Vector3(maxWidth / -2f, 0, maxHeight / -2f);
            _gridOrigin.x = transform.position.x;
            _gridOrigin.y = transform.position.z;

            _cells = new Dictionary<Vector2Int, Tower>();
            var columns = Mathf.FloorToInt(maxWidth / cellSize);
            var rows = Mathf.FloorToInt(maxHeight / cellSize);

            for (var x = 0; x < columns; x++)
            for (var y = 0; y < rows; y++)
                _cells.Add(new Vector2Int(x, y), null);
        }

        public Vector2Int WorldToCell(Vector3 worldPos)
        {
            var x = Mathf.FloorToInt((worldPos.x - _gridOrigin.x) / cellSize);
            var y = Mathf.FloorToInt((worldPos.z - _gridOrigin.y) / cellSize);
            return new Vector2Int(x, y);
        }

        public Vector3 CellToWorld(Vector2Int cell)
        {
            return new Vector3(
                _gridOrigin.x + cell.x * cellSize + cellSize * 0.5f,
                transform.position.y,
                _gridOrigin.y + cell.y * cellSize + cellSize * 0.5f
            );
        }

        public bool IsValid(Vector2Int cell)
        {
            return _cells.ContainsKey(cell) && _cells[cell] is null;
        }
    }
}