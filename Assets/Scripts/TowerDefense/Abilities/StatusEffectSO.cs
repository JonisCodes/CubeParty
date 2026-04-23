using UnityEngine;

namespace TowerDefense.Abilities
{
    [CreateAssetMenu(fileName = "Status Effect", menuName = "Status Effect/New", order = 0)]
    public class StatusEffectSO : ScriptableObject
    {
        [Header("General")] public bool HasDuration;

        public ElementTag EffectTag;
        public float Duration;

        [Header("Stack")] public bool IsStacking;
        public bool DecayStacks;
        public int maxStacks;

        [Header("Tick")] public float TickRate;
        public float DamagePerTick;

        [Header("UI")] public Sprite Icon;
        public GameObject uiPrefab;
        public Color BackgroundColor;
    }

    public enum ElementTag
    {
        Fire,
        Frost,
        Chemical,
        Physical,
        Soaked
    }
}