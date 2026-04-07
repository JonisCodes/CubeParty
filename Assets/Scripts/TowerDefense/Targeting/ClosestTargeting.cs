using System.Collections.Generic;
using TowerDefense.AI;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Targeting
{
    [CreateAssetMenu(fileName = "closestTarget", menuName = "Targeting/Closest Target", order = 0)]
    public class ClosestTargeting : TargetingStrategy
    {
        public override Enemy GetTarget(Tower tower, List<Enemy> enemies)
        {
            Enemy closest = null;
            var minDistance = float.MaxValue;

            foreach (var enemy in enemies)
            {
                var distance = Vector3.Distance(tower.transform.position, enemy.transform.position);

                if (!(distance < minDistance) || !(distance <= tower.Range)) continue;
                
                minDistance = distance;
                closest = enemy;
            }

            return closest;
        }
    }
}