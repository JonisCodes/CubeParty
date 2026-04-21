using TowerDefense.AI;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Abilities
{
    [CreateAssetMenu(fileName = "basicRanged", menuName = "Abilities/Basic Ranged", order = 0)]
    public class BasicRanged : Ability
    {
        public override void Execute(Tower tower, Enemy target)
        {
            if (target is null) return;

            target.TakeDamage(tower.Damage);
            ApplyStatusEffects(target);

            Debug.DrawLine(tower.transform.position, target.transform.position, Color.green, 1f, false);
        }

        protected override void ApplyStatusEffects(Enemy enemy)
        {
            enemy.AddStatus(StatusEffect, GetStacks(enemy));
        }
    }
}