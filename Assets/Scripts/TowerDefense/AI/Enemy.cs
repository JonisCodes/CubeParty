using System.Collections.Generic;
using System.Linq;
using TowerDefense.Abilities;
using TowerDefense.Interfaces;
using TowerDefense.Managers;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.AI
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform uiRoot;
        [SerializeField] private Canvas stackCanvas;
        [SerializeField] private float xpOnDeath = 5f;

        private readonly Dictionary<StatusEffectSO, StatusInstance> _statuses = new();
        private readonly Dictionary<StatusEffectSO, GameObject> _statusUIs = new();

        public float Health { get; set; } = 100f;

        public float PathProgress { get; set; }

        private void Awake()
        {
            stackCanvas.worldCamera = Camera.main;
        }

        private void Update()
        {
            UpdateCanvasRotation();

            var dt = Time.deltaTime;

            foreach (var status in _statuses.Values) status.Tick(dt, this);

            CleanupStatuses();
        }

        public void TakeDamage(float damage, IDamageSource src)
        {
            Health -= damage;
            print($"Damage taken: {damage} from {src.DisplayName}");
            if (!(Health <= 0f)) return;

            OnDeath(src);
        }

        public void ApplyStatus(StatusEffectSO effect, IDamageSource source, int stacks = 1)
        {
            if (effect is null || source is null) return;

            AddStatus(effect, stacks, source);
        }

        public Vector3 Position => transform.position;

        private void CleanupStatuses()
        {
            var toRemove = (from pair in _statuses where pair.Value.IsExpired() select pair.Key).ToList();

            foreach (var key in toRemove)
            {
                _statuses.Remove(key);

                if (!_statusUIs.TryGetValue(key, out var ui)) continue;

                Destroy(ui);
                _statusUIs.Remove(key);
            }
        }

        public bool IsInRange(Tower tower)
        {
            return Vector3.Distance(transform.position, tower.transform.position) <= tower.Range;
        }

        public bool HasTag(ElementTag effectTag)
        {
            return _statuses.Values.Any(value => value.Definition.effectTag == effectTag);
        }

        public void AddStatus(StatusEffectSO status, int stacks, IDamageSource source)
        {
            if (!_statuses.TryGetValue(status, out var instance))
            {
                instance = new StatusInstance(status, source);
                _statuses[status] = instance;

                var statusUI = status.CreateUI(instance, uiRoot);
                _statusUIs[status] = statusUI;
            }

            instance.AddStacks(stacks);
        }

        private void OnDeath(IDamageSource deathSource)
        {
            if (deathSource.Owner.TryGetComponent<IXpReceiver>(out var xpReceiver))
                xpReceiver.AddXp(xpOnDeath);

            WaveManager.Instance.OnEnemyRemoved(this);
            print("enemy dead");
            Destroy(gameObject);
        }

        private void UpdateCanvasRotation()
        {
            uiRoot.transform.LookAt(transform.position + stackCanvas.worldCamera.transform.rotation * Vector3.forward,
                stackCanvas.worldCamera.transform.rotation * Vector3.up);
        }
    }
}