using System.Collections.Generic;
using TowerDefense.Upgrades;
using UnityEngine;

namespace TowerDefense.Managers
{
    public class UpgradeDatabase : MonoBehaviour
    {
        public List<TowerUpgrade> allUpgrades;
        public static UpgradeDatabase Instance { get; private set; }


        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public TowerUpgrade GetRandomUpgrade()
        {
            if (allUpgrades is null || allUpgrades.Count == 0) return null;

            return allUpgrades[Random.Range(0, allUpgrades.Count)];
        }
    }
}