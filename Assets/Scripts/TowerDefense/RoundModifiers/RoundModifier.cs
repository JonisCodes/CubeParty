using TowerDefense.Managers;
using UnityEngine;

namespace TowerDefense.RoundModifiers
{
    [CreateAssetMenu(fileName = "RoundModifier")]
    public abstract class RoundModifier : ScriptableObject
    {
        [Header("Modifier Info")] public string modifierName;
        public string description;
        public Sprite icon;

        [Header("Settings")] public float weight = 1f;

        public abstract void Apply(RoundManager roundManager);
        public abstract void Remove(RoundManager roundManager);
    }
}