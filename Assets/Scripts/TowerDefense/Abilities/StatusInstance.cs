using TowerDefense.AI;
using UnityEngine;

namespace TowerDefense.Abilities
{
    public class StatusInstance
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

        public void Tick(float dt, Enemy enemy)
        {
            if (Definition.TickRate > 0f)
            {
                _tickTimer += dt;
                if (_tickTimer >= Definition.TickRate)
                {
                    _tickTimer = 0f;
                    enemy.TakeDamage(Stacks * Definition.DamagePerTick);
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
        }
    }
}