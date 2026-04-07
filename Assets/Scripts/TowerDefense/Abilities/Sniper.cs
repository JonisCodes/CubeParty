using TowerDefense.AI;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Abilities
{
    [CreateAssetMenu(fileName = "Sniper", menuName = "Abilities/Sniper", order = 0)]
    public class Sniper : Ability
    {
        public override void Execute(Tower tower, Enemy target)
        {
            if (tower is null || target is null) return;

            target.TakeDamage(tower.Damage);
            Debug.DrawLine(tower.transform.position, target.transform.position, Color.darkBlue, 1f, false);
        }
    }
}