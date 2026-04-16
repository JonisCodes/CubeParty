using UnityEngine;

namespace TowerDefense.Managers
{
    public class TowerManager : MonoBehaviour
    {
        private PlacementGrid _placementGrid;
        public static TowerManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _placementGrid = FindFirstObjectByType<PlacementGrid>();
        }

        public bool PlaceTower(GameObject prefab, Vector2Int cell)
        {
            if (_placementGrid is null) return false;

            if (prefab is null) return false;

            if (!_placementGrid.IsValid(cell)) return false;

            var worldPos = _placementGrid.CellToWorld(cell);
            Instantiate(prefab, worldPos, Quaternion.identity);

            return true;
        }
    }
}