using System.Collections.Generic;
using System.Linq;
using TowerDefense.Abilities;
using TowerDefense.Enums;
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

        private readonly List<IStatModifier> _modifiers = new();
        private List<AbilityInstance> _abilities;
        private float _currentCooldown;
        private bool _isBatching;

        public float Range { get; set; }
        public float AttackSpeed { get; set; }
        public float Health { get; set; }
        public int Level { get; private set; } = 1;
        public float CurrentXP { get; private set; }

        private void Awake()
        {
            InitializeTower();
        }

        private void Update()
        {
            foreach (var ability in _abilities)
            {
                ability.Tick(Time.deltaTime);

                var target = data.targeting.GetTarget(this, WaveManager.Instance.CurrentEnemies);

                if (target is null)
                    continue;

                ability.TryExecute(this, target);
            }

            // _currentCooldown -= Time.deltaTime;
            //
            // if (_currentCooldown > 0f) return;
            //
            // Attack();
            // _currentCooldown = 1f / data.attackSpeed;
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

        private void InitializeTower()
        {
            SetBaseStats();
            InitializeAbilities();
            RecalculateStats();
        }

        private void SetBaseStats()
        {
            Range = data.baseRange;
            AttackSpeed = data.attackSpeed;
            _currentCooldown = 0;
        }

        private void InitializeAbilities()
        {
            _abilities = new List<AbilityInstance>();

            BeginModifierBatch();

            foreach (var ability in data.abilities)
            {
                var instance = new AbilityInstance(ability);
                BindAbility(instance);
                _abilities.Add(instance);
            }

            foreach (var ability in _abilities)
                ability.Equip(this);

            EndModifierBatch();
        }

        private void Attack()
        {
            if (WaveManager.Instance is null) return;

            var target = data.targeting.GetTarget(this, WaveManager.Instance.CurrentEnemies);

            if (target is null)
                // Debug.LogError("There is no targeting for this tower");
                return;

            foreach (var ability in _abilities) ability.Execute(this, target);
        }

        private void BindAbility(AbilityInstance ability)
        {
            ability.OnExecute += context =>
            {
                if (context.Target is null) return;

                context.Target.TakeDamage(context.BaseDamage * context.PowerMultiplier, context.Source);

                foreach (var effect in context.OnHitEffects) context.Target.ApplyStatus(effect, context.Source);
            };
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

        public void AddStatModifier(IStatModifier statModifier)
        {
            _modifiers.Add(statModifier);

            if (!_isBatching)
                RecalculateStats();
        }

        public void BeginModifierBatch()
        {
            _isBatching = true;
        }

        public void EndModifierBatch()
        {
            _isBatching = false;
            RecalculateStats();
        }

        private void RecalculateStats()
        {
            // Damage = ApplyModifiers(StatType.Damage, data.damage);
            Range = ApplyModifiers(StatType.Range, data.baseRange);
            AttackSpeed = ApplyModifiers(StatType.AttackSpeed, data.attackSpeed);
        }

        private float ApplyModifiers(StatType statType, float baseValue)
        {
            return _modifiers.Where(mod => mod.Stat == statType)
                .Aggregate(baseValue, (current, mod) => mod.Apply(current));
        }

        public void RemoveStatModifier(IStatModifier statModifier)
        {
            _modifiers.Remove(statModifier);

            if (!_isBatching)
                RecalculateStats();
        }
    }
}