using UnityEngine;

namespace TowerDefense.Interfaces
{
    public interface IDamageSource
    {
        string DisplayName { get; }
        GameObject Owner { get; }
    }
}