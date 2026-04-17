using System.Linq;
using TowerDefense.Towers;
using UnityEngine;

[ExecuteInEditMode]
public class GizmoHandler : MonoBehaviour
{
    private Tower _tower;

    private void Awake()
    {
        _tower = GetComponent<Tower>();
    }

    private void OnDrawGizmos()
    {
        if (_tower is null)
            _tower = GetComponent<Tower>();

        if (_tower is null)
        {
            Debug.LogWarning("Tower not found");
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_tower.GetPosition(), GetTotalRange());
    }

    private float GetTotalRange()
    {
        if (_tower is null) return float.MinValue;

        return _tower.data.abilities.Aggregate(_tower.data.baseRange,
            (current, ability) => current * ability.statModifier.rangeMultiplier);
    }
}