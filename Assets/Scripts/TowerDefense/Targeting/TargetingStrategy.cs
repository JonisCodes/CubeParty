using System.Collections.Generic;
using System.Linq;
using TowerDefense.AI;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Targeting
{
    [CreateAssetMenu(fileName = "TargetingStrategy")]
    public abstract class TargetingStrategy : ScriptableObject
    {
        protected List<Enemy> FilterInRange(Tower tower, List<Enemy> enemies)
        {
            return enemies.Where(e => Vector3.Distance(tower.GetPosition(), e.transform.position) <= tower.Range)
                .ToList();
        }

        public abstract Enemy GetTarget(Tower tower, List<Enemy> enemies);
    }
}