using System.Collections.Generic;
using System.Linq;
using TowerDefense.RoundModifiers;
using UnityEngine;
using UnityEngine.Serialization;

namespace TowerDefense.Managers
{
    public class RoundManager : MonoBehaviour
    {
        [Header("Modifiers")] public List<RoundModifier> allModifiers;

        public int modifiersPerRound = 1;

        [FormerlySerializedAs("_activeModifiers")] [SerializeField]
        private List<RoundModifier> activeModifiers = new();

        private int _currentRound;
        public static RoundManager Instance { get; private set; }

        public float EnemySpeedMultiplier { get; set; } = 1f;
        public float EnemyHealthMultiplier { get; set; } = 1f;
        public int SpawnCountMultiplier { get; set; } = 1;


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Start()
        {
            StartRound();
        }

        private void StartRound()
        {
            _currentRound++;
            RemoveActiveModifiers();
            ResetMultipliers();
            PickAndApplyModifiers();
            WaveManager.Instance.StartWave();
        }

        public void OnRoundComplete()
        {
            print($"Round {_currentRound} has been completed.");
        }

        private void ResetMultipliers()
        {
            EnemySpeedMultiplier = 1f;
            SpawnCountMultiplier = 1;
        }

        private void PickAndApplyModifiers()
        {
            var pool = new List<RoundModifier>(allModifiers);
            activeModifiers.Clear();

            for (var i = 0; i < modifiersPerRound && pool.Count > 0; i++)
            {
                var chosen = WeightedRandom(pool);
                chosen.Apply(this);
                activeModifiers.Add(chosen);
                pool.Remove(chosen);
            }
        }

        private void RemoveActiveModifiers()
        {
            foreach (var mod in activeModifiers)
                mod.Remove(this);
            activeModifiers.Clear();
        }

        private static RoundModifier WeightedRandom(List<RoundModifier> pool)
        {
            var totalWeight = pool.Sum(m => m.weight);
            var roll = Random.Range(0f, totalWeight);
            var cumulative = 0f;

            foreach (var mod in pool)
            {
                cumulative += mod.weight;
                if (roll <= cumulative) return mod;
            }

            return pool[^1];
        }
    }
}