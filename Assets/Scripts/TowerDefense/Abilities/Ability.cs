using System.Collections.Generic;
using TowerDefense.AI;
using TowerDefense.Towers.TowerStatModifiers;
using UnityEngine;

namespace TowerDefense.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [Header("Ability Info")] public string abilityName;

        public string description;
        public Sprite icon;

        public float cooldown;
        public float baseScalingPerLevel = 1;

        public float damage;

        [Header("Passive Modifiers")] public List<TowerStatModifier> statModifiers;

        public List<StatusEffectSO> onHitEffects = new();

        protected virtual int GetStacks(Enemy enemy)
        {
            return 1;
        }
    }
}