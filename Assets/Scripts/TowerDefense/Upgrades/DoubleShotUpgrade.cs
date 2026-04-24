using TowerDefense.Abilities.Execution.Modifiers;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/Double Shot", order = 0)]
    public class DoubleShotUpgrade : TowerUpgrade
    {
        public override void Apply(Tower tower)
        {
            foreach (var ability in tower.Abilities)
                ability.AddExecutionModifier(new DoubleShotModifier());
        }
    }
}