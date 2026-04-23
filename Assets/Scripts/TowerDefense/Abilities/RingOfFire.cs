using TowerDefense.AI;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Abilities
{
    [CreateAssetMenu(fileName = "RingOfFire", menuName = "Abilities/Ring of Fire", order = 0)]
    public class RingOfFire : Ability
    {
        public LayerMask layerMask;

        public override void Execute(Tower tower, Enemy target)
        {
            var results = new Collider[64];
            var hitCount = Physics.OverlapSphereNonAlloc(tower.transform.position, tower.Range, results,
                layerMask);

            for (var i = 0; i < hitCount; i++)
                if (results[i].TryGetComponent<Enemy>(out var hitEnemy))
                {
                    hitEnemy.TakeDamage(tower.Damage, tower);
                    ApplyStatusEffects(hitEnemy, tower);
                }
        }

        protected override void ApplyStatusEffects(Enemy enemy, Tower tower)
        {
            enemy.AddStatus(StatusEffect, GetStacks(enemy), tower);
        }
    }
}