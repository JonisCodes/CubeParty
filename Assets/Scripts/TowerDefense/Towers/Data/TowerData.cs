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
        [Tooltip("Attacks per second")] public float attackSpeed;
        public List<Ability> abilities;
        public TargetingStrategy targeting;
    }
}