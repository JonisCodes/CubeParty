using TowerDefense.AI;
using TowerDefense.Towers;
using TowerDefense.Towers.TowerStatModifiers;
using UnityEngine;

namespace TowerDefense.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [Header("Ability Info")] 
        public string abilityName;
        public string description;
        public Sprite icon;
        
        [Header("Passive Modifiers")]
        public TowerStatModifier statModifier;
        
        public abstract void Execute(Tower tower, Enemy target);

        public virtual void SetupModifiers(Tower tower)
        {
            statModifier?.Apply(tower);
        }

        public virtual void RemoveModifiers(Tower tower)
        {
            statModifier?.Remove(tower);
        }
    }
}