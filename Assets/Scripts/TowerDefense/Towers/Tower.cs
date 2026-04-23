using TowerDefense.Interfaces;
using TowerDefense.Managers;
using TowerDefense.Towers.Data;
using UnityEngine;

namespace TowerDefense.Towers
{
    public class Tower : MonoBehaviour,
        IInteraction,
        IDamageSource,
        IXpReceiver
    {
        [SerializeField] private Transform selectedRingTransform;

        public TowerData data;

        [SerializeField] private Vector3 offset;

        [SerializeField] private float xpToNextLevel = 10f;
        private float _currentCooldown;

        public float Range { get; set; }
        public float Damage { get; set; }
        public float AttackSpeed { get; set; }
        public float Health { get; set; }
        public int Level { get; private set; } = 1;
        public float CurrentXP { get; private set; }

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

        public string DisplayName => gameObject.name;
        public GameObject Owner => gameObject;


        public void Interact()
        {
            selectedRingTransform.gameObject.SetActive(true);
        }

        public void Uninteract()
        {
            selectedRingTransform.gameObject.SetActive(false);
        }

        public void AddXp(float amount)
        {
            CurrentXP += amount;
            if (CurrentXP >= xpToNextLevel)
                LevelUp();
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

        public Vector3 GetPosition()
        {
            return selectedRingTransform.position + offset;
        }

        private void LevelUp()
        {
            Level++;
            CurrentXP = 0;
            print($"Level Up : {Level}");

            ApplyLevelStats();
        }

        private void ApplyLevelStats()
        {
        }
    }
}