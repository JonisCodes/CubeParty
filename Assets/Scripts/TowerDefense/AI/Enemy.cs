using System.Collections.Generic;
using System.Linq;
using TowerDefense.Abilities;
using TowerDefense.Interfaces;
using TowerDefense.Managers;
using TowerDefense.Towers;
using TowerDefense.UI;
using UnityEngine;

namespace TowerDefense.AI
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform uiRoot;
        [SerializeField] private Canvas stackCanvas;

        private readonly Dictionary<StatusEffectSO, StatusInstance> _statuses = new();
        private readonly Dictionary<StatusEffectSO, GameObject> _statusUIs = new();

        public float Health { get; set; } = 100f;

        public float PathProgress { get; set; }

        private void Update()
        {
            var dt = Time.deltaTime;

            foreach (var status in _statuses.Values) status.Tick(dt, this);

            CleanupStatuses();
        }

        private void CleanupStatuses()
        {
            var toRemove = (from pair in _statuses where pair.Value.IsExpired() select pair.Key).ToList();

            foreach (var key in toRemove)
            {
                _statuses.Remove(key);

                if (_statusUIs.TryGetValue(key, out var ui))
                {
                    Destroy(ui);
                    _statusUIs.Remove(key);
                }
            }
        }

        public bool IsInRange(Tower tower)
        {
            return Vector3.Distance(transform.position, tower.transform.position) <= tower.Range;
        }

        public bool HasTag(ElementTag effectTag)
        {
            return _statuses.Values.Any(value => value.Definition.EffectTag == effectTag);
        }

        public void AddStatus(StatusEffectSO status, int stacks)
        {
            if (!_statuses.TryGetValue(status, out var instance))
            {
                instance = new StatusInstance(status);
                _statuses[status] = instance;

                var go = Instantiate(status.uiPrefab, uiRoot);
                go.GetComponent<StackUI>().Init(instance, stackCanvas);
                _statusUIs[status] = go;
            }

            instance.AddStacks(stacks);
        }

        public void TakeDamage(float damage, IDamageSource src)
        {
            Health -= damage;
            print($"Damage taken: {damage} from {src.DisplayName}");
            if (!(Health <= 0f)) return;

            print("enemy dead");
            WaveManager.Instance.OnEnemyRemoved(this);
            Destroy(gameObject);
        }
    }
}