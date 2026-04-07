using TowerDefense.Managers;
using UnityEngine;

namespace TowerDefense.RoundModifiers
{
    [CreateAssetMenu(fileName = "Tankier Enemies Modifier", menuName = "Round Modifiers/Tankier Enemies", order = 0)]
    public class TankierEnemies : RoundModifier
    {
        public float healthModifier = 1f;

        public override void Apply(RoundManager roundManager)
        {
            roundManager.EnemyHealthMultiplier += healthModifier;
        }

        public override void Remove(RoundManager roundManager)
        {
            roundManager.EnemyHealthMultiplier -= healthModifier;
        }
    }
}