using TowerDefense.Interfaces;
using TowerDefense.Managers;
using UnityEngine;

namespace TowerDefense.Towers
{
    public class Tower : MonoBehaviour, IInteraction
    {
        [SerializeField] private Transform selectedRingTransform;

        public TowerData.TowerData data;
        private float _currentCooldown;

        public float Range { get; set; }
        public float Damage { get; set; }
        public float AttackSpeed { get; set; }
        public float Health { get; set; }

        private void Awake()
        {
            Range = data.baseRange;
            Damage = data.damage;
            AttackSpeed = data.attackSpeed;

            _currentCooldown = 0;

            foreach (var ability in data.abilities) ability.SetupModifiers(this);
        }

        private void Update()
        {
            _currentCooldown -= Time.deltaTime;

            if (_currentCooldown > 0f) return;

            Attack();
            _currentCooldown = 1f / data.attackSpeed;
        }


        public void Interact()
        {
            selectedRingTransform.gameObject.SetActive(true);
        }

        public void Uninteract()
        {
            selectedRingTransform.gameObject.SetActive(false);
        }

        private void Attack()
        {
            if (WaveManager.Instance is null) return;

            var target = data.targeting.GetTarget(this, WaveManager.Instance.CurrentEnemies);

            if (target is null)
                // Debug.LogError("There is no targeting for this tower");
                return;

            foreach (var ability in data.abilities) ability.Execute(this, target);
        }

        public void ResetStats()
        {
            Damage = data.damage;
            AttackSpeed = data.attackSpeed;
            Range = data.baseRange;
        }
    }
}