using TowerDefense.Enums;
using TowerDefense.Interfaces;
using UnityEngine;

namespace TowerDefense.Towers.TowerStatModifiers
{
    [CreateAssetMenu(fileName = "Tower Stat Modifier", menuName = "Tower Stat/Modifier", order = 0)]
    public class TowerStatModifier : ScriptableObject, IStatModifier
    {
        public float value = 1f;
        public ModifierType type;

        public StatType stat;
        public StatType Stat => stat;

        public float Apply(float baseValue)
        {
            return type switch
            {
                ModifierType.Multiply => baseValue * value,
                ModifierType.Add => baseValue + value,
                _ => baseValue
            };
        }
    }
}