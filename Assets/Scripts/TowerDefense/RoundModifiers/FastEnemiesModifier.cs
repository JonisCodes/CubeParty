using TowerDefense.Managers;
using UnityEngine;

namespace TowerDefense.RoundModifiers
{
    [CreateAssetMenu(fileName = "FastEnemiesModifier", menuName = "Round Modifiers/Fast Enemies", order = 0)]
    public class FastEnemiesModifier : RoundModifier
    {
        public float speedMultiplier = 1.5f;

        public override void Apply(RoundManager roundManager)
        {
            roundManager.EnemySpeedMultiplier *= speedMultiplier;
        }

        public override void Remove(RoundManager roundManager)
        {
            roundManager.EnemySpeedMultiplier /= speedMultiplier;
        }
    }
}