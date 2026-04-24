using UnityEngine;

namespace TowerDefense.Abilities
{
    [CreateAssetMenu(fileName = "RingOfFire", menuName = "Abilities/Ring of Fire", order = 0)]
    public class RingOfFire : Ability
    {
        public LayerMask layerMask;
    }
}