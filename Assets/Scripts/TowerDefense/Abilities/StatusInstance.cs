using System;
using TowerDefense.AI;
using TowerDefense.Interfaces;
using UnityEngine;

namespace TowerDefense.Abilities
{
    public class StatusInstance : IDamageSource
    {
        public readonly StatusEffectSO Definition;
        private float _duration;
        private float _tickTimer;
        public int MaxStacks;
        public int Stacks;


        public StatusInstance(StatusEffectSO definition, IDamageSource source)
        {
            Definition = definition;
            MaxStacks = definition.maxStacks;
            _duration = definition.hasDuration ? definition.duration : float.MaxValue;
            _tickTimer = 0f;
            Stacks = 0;
            Owner = source.Owner;
        }

        public string DisplayName => $"{Owner.name}: {Definition.name}";
        public GameObject Owner { get; }

        public event Action<int> OnStacksChanged;

        public void Tick(float dt, Enemy enemy)
        {
            if (Definition.tickRate > 0f)
            {
                _tickTimer += dt;
                if (_tickTimer >= Definition.tickRate)
                {
                    _tickTimer = 0f;
                    enemy.TakeDamage(Stacks * Definition.damagePerTick, this);
                }
            }

            _duration -= dt;
        }

        public bool IsExpired()
        {
            return _duration <= 0f || Stacks <= 0;
        }

        public void AddStacks(int amount)
        {
            Stacks = Mathf.Clamp(Stacks + amount, 0, MaxStacks);
            OnStacksChanged?.Invoke(Stacks);
        }
    }
}