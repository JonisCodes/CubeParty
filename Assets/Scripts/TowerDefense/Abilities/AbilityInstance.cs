using System;
using System.Collections.Generic;
using TowerDefense.Interfaces;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Abilities
{
    public class AbilityInstance
    {
        private readonly Ability _definition;
        private readonly List<StatusEffectSO> _onHitEffects = new();

        private int _level;

        public AbilityInstance(Ability definition)
        {
            _definition = definition;

            if (_definition.onHitEffects is not null)
                _onHitEffects = new List<StatusEffectSO>(_definition.onHitEffects);
        }

        public float CooldownRemaining { get; private set; }

        public void AddOnHitEffect(StatusEffectSO effect)
        {
            _onHitEffects.Add(effect);
        }

        public void Tick(float dt)
        {
            if (CooldownRemaining > 0f)
                CooldownRemaining -= dt;
        }

        public void TryExecute(IDamageSource caster, IDamageable target)
        {
            if (CooldownRemaining > 0f)
                return;

            Execute(caster, target);

            CooldownRemaining = _definition.cooldown;
        }

        public event Action<AbilityExecutionContext> OnExecute;

        public void Execute(IDamageSource caster, IDamageable target)
        {
            if (CooldownRemaining > 0f)
                return;

            var context = new AbilityExecutionContext
            {
                Caster = caster.Owner,
                Target = target,
                Origin = caster.Owner.transform.position,
                Source = caster,
                PowerMultiplier = GetPowerScaling(),
                OnHitEffects = _onHitEffects,
                BaseDamage = _definition.damage
            };

            OnExecute?.Invoke(context);

            CooldownRemaining = _definition.cooldown;
        }

        private float GetPowerScaling()
        {
            return 1f + _definition.baseScalingPerLevel * _level;
        }

        public void Equip(Tower tower)
        {
            foreach (var mod in _definition.statModifiers)
                tower.AddStatModifier(mod);
        }
    }

    public struct AbilityExecutionContext
    {
        public GameObject Caster;
        public IDamageable Target;

        public Vector3 Origin;
        public IDamageSource Source;

        public float PowerMultiplier;

        public List<StatusEffectSO> OnHitEffects;

        public float BaseDamage;
    }
}