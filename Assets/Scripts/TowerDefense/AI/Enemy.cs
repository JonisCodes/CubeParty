using System.Collections.Generic;
using System.Linq;
using TowerDefense.Abilities;
using TowerDefense.Managers;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.AI
{
    public class Enemy : MonoBehaviour
    {
        private readonly Dictionary<StatusEffectSO, StatusInstance> _statuses = new();
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

            foreach (var key in toRemove) _statuses.Remove(key);
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
            }

            instance.AddStacks(stacks);
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            print($"Damage taken: {damage}");
            if (!(Health <= 0f)) return;

            print("enemy dead");
            WaveManager.Instance.OnEnemyRemoved(this);
            Destroy(gameObject);
        }
    }
}