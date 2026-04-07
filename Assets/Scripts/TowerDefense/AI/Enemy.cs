using TowerDefense.Managers;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.AI
{
    public class Enemy : MonoBehaviour
    {
        public float Health { get; set; } = 100f;

        public float PathProgress { get; set; }

        public bool IsInRange(Tower tower)
        {
            return Vector3.Distance(transform.position, tower.transform.position) <= tower.Range;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            print($"Damage taken: {damage}");
            if (!(Health <= 0f)) return;

            print("enemy dead");
            WaveManager.Instance.OnEnemyRemoved(this);
            Destroy(gameObject);
        }
    }
}