using System.Collections.Generic;
using TowerDefense.Abilities;
using TowerDefense.Targeting;
using UnityEngine;

namespace TowerDefense.Towers.Data
{
    [CreateAssetMenu(fileName = "towerData", menuName = "Tower/Data", order = 0)]
    public class TowerData : ScriptableObject
    {
        public float damage;
        public float baseRange;
        public float growthExponent = 1.5f;
        public float baseXp = 10f;
        [Tooltip("Attacks per second")] public float attackSpeed;
        public List<Ability> abilities;
        public TargetingStrategy targeting;
    }
}