using TowerDefense.Managers;
using UnityEngine;

namespace TowerDefense.RoundModifiers
{
    [CreateAssetMenu(fileName = "DoubleEnemiesModifier", menuName = "Round Modifiers/Double Enemies", order = 1)]
    public class DoubleEnemiesModifier : RoundModifier
    {
        public override void Apply(RoundManager roundManager)
        {
            roundManager.SpawnCountMultiplier *= 2;
        }

        public override void Remove(RoundManager roundManager)
        {
            roundManager.SpawnCountMultiplier /= 2;
        }
    }
}