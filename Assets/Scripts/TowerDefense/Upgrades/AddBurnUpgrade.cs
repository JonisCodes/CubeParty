using TowerDefense.Abilities;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/Burn Upgrade", order = 0)]
    public class AddBurnUpgrade : TowerUpgrade
    {
        public StatusEffectSO burnEffect;

        public override void Apply(Tower tower)
        {
            foreach (var ability in tower.Abilities) ability.AddOnHitEffect(burnEffect);
        }
    }
}