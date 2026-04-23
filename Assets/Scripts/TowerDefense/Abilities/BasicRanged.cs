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

            target.TakeDamage(tower.Damage, tower);
            ApplyStatusEffects(target, tower);

            Debug.DrawLine(tower.transform.position, target.transform.position, Color.green, 1f, false);
        }

        protected override void ApplyStatusEffects(Enemy enemy, Tower tower)
        {
            base.ApplyStatusEffects(enemy, tower);
            enemy.AddStatus(StatusEffect, GetStacks(enemy), tower);
        }
    }
}