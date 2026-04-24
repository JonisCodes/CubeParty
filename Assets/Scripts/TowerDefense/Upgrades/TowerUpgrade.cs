using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Upgrades
{
    public abstract class TowerUpgrade : ScriptableObject
    {
        public int tier;
        public float powerWeight;
        public abstract void Apply(Tower tower);
    }
}