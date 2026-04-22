using System;
using TowerDefense.AI;
using TowerDefense.Interfaces;
using UnityEngine;

namespace TowerDefense.Abilities
{
    public class StatusInstance : IDamageSource
    {
        private float _duration;
        private float _tickTimer;
        public StatusEffectSO Definition;
        public int MaxStacks;
        public int Stacks;


        public StatusInstance(StatusEffectSO definition)
        {
            Definition = definition;
            MaxStacks = definition.maxStacks;
            _duration = definition.HasDuration ? definition.Duration : float.MaxValue;
            _tickTimer = 0f;
            Stacks = 0;
        }

        public string DisplayName => Definition.name;

        public event Action<int> OnStacksChanged;

        public void Tick(float dt, Enemy enemy)
        {
            if (Definition.TickRate > 0f)
            {
                _tickTimer += dt;
                if (_tickTimer >= Definition.TickRate)
                {
                    _tickTimer = 0f;
                    enemy.TakeDamage(Stacks * Definition.DamagePerTick, this);
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