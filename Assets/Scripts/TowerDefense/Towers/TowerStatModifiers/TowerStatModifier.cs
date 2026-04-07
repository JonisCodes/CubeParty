using UnityEngine;

namespace TowerDefense.Towers.TowerStatModifiers
{
    [CreateAssetMenu(fileName = "Tower Stat Modifier", menuName = "Tower Stat/Modifier", order = 0)]
    public class TowerStatModifier : ScriptableObject
    {
        public float rangeMultiplier = 1f;
        public float damageMultiplier = 1f;
        public float attackSpeedMultiplier = 1f;
        public float healthMultiplier = 1f;

        public void Apply(Tower tower)
        {
            if (tower is null)
            {
                Debug.LogWarning("Tower Stat Modifier: tower is null");
                return;
            }
            
            tower.Range *= rangeMultiplier;
            tower.AttackSpeed *= attackSpeedMultiplier;
            tower.Damage *= damageMultiplier;
            tower.Health *= healthMultiplier;
        }

        public void Remove(Tower tower)
        {
            if (tower is null)
            {
                Debug.LogWarning("Tower Stat Modifier: tower is null");
                return;
            }
            
            tower.Range /= rangeMultiplier;
            tower.AttackSpeed /= attackSpeedMultiplier;
            tower.Damage /= damageMultiplier;
            tower.Health /= healthMultiplier;
        }
    }
}