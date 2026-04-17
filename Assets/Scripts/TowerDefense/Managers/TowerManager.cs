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

        private static Bounds GetObjectBounds(GameObject obj)
        {
            var renderers = obj.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return new Bounds(obj.transform.position, Vector3.zero);
            var bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
                bounds.Encapsulate(renderers[i].bounds);
            return bounds;
        }

        public bool PlaceTower(GameObject prefab, Vector2Int cell)
        {
            if (_placementGrid is null) return false;

            if (prefab is null) return false;

            if (!_placementGrid.IsValid(cell)) return false;

            var worldPos = _placementGrid.CellToWorldSurface(cell);
            var instance = Instantiate(prefab, worldPos, Quaternion.identity);

            var bounds = GetObjectBounds(instance);
            var bottomOffset = instance.transform.position.y - bounds.min.y;
            instance.transform.position += Vector3.up * bottomOffset;

            return true;
        }
    }
}