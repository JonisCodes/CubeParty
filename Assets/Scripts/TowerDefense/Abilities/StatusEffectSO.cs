using TowerDefense.UI;
using UnityEngine;

namespace TowerDefense.Abilities
{
    [CreateAssetMenu(fileName = "Status Effect", menuName = "Status Effect/New", order = 0)]
    public class StatusEffectSO : ScriptableObject
    {
        [Header("General")] public bool hasDuration;

        public ElementTag effectTag;
        public float duration;

        [Header("Stack")] public bool isStacking;
        public bool decayStacks;
        public int maxStacks;

        [Header("Tick")] public float tickRate;
        public float damagePerTick;

        [Header("UI")] public Sprite icon;
        public StackUI uiPrefab;
        public Color backgroundColor;

        public virtual GameObject CreateUI(StatusInstance instance, Transform parent)
        {
            if (uiPrefab is null) return null;
            var ui = Instantiate(uiPrefab, parent);
            ui.Init(instance);
            return ui.gameObject;
        }
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