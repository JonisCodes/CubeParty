using System.Collections.Generic;
using System.Linq;
using TowerDefense.AI;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Targeting
{
    [CreateAssetMenu(fileName = "firstEnemy", menuName = "Targeting/First Enemy", order = 0)]
    public class FirstEnemy : TargetingStrategy
    {
        public override Enemy GetTarget(Tower tower, List<Enemy> enemies)
        {
            Enemy best = null;
            var maxProgress = float.MinValue;

            foreach (var enemy in enemies.Where(enemy => enemy.IsInRange(tower)).Where(enemy => !(enemy.PathProgress < maxProgress)))
            {
                maxProgress = enemy.PathProgress;
                best = enemy;
            }

            return best;
        }
    }
}