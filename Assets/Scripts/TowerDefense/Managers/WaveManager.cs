using System.Collections;
using System.Collections.Generic;
using TowerDefense.AI;
using UnityEngine;

namespace TowerDefense.Managers
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private GameObject aiPrefab;
        [SerializeField] private int baseAmountToSpawn = 5;
        [SerializeField] private Spline spline;
        [SerializeField] private float timeBetweenSpawns = 1f;


        private int _enemiesAlive;
        public static WaveManager Instance { get; private set; }
        public List<EnemyMovement> EnemyMovements { get; private set; }

        public List<Enemy> CurrentEnemies { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void StartWave()
        {
            PrepareEnemies();
            StartCoroutine(SpawnEnemies());
        }

        private void PrepareEnemies()
        {
            EnemyMovements = new List<EnemyMovement>();
            CurrentEnemies = new List<Enemy>();
            var totalToSpawn = baseAmountToSpawn * RoundManager.Instance.SpawnCountMultiplier;

            for (var i = 0; i < totalToSpawn; i++)
            {
                var newEnemy = Instantiate(aiPrefab);

                var enemy = newEnemy.GetComponent<Enemy>();
                CurrentEnemies.Add(enemy);
                enemy.Health *= RoundManager.Instance.EnemyHealthMultiplier;

                var movement = newEnemy.GetComponent<EnemyMovement>();
                movement.Initialize(spline);
                movement.SetSpeed(RoundManager.Instance.EnemySpeedMultiplier);
                newEnemy.SetActive(false);
                EnemyMovements.Add(movement);
            }

            _enemiesAlive = totalToSpawn;
        }

        private IEnumerator SpawnEnemies()
        {
            while (EnemyMovements.Count > 0)
            {
                var lastIndex = EnemyMovements.Count - 1;
                var movement = EnemyMovements[lastIndex];

                EnemyMovements.RemoveAt(lastIndex);

                movement.gameObject.SetActive(true);

                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }

        public void OnEnemyRemoved(Enemy enemy)
        {
            _enemiesAlive--;
            CurrentEnemies.Remove(enemy);
            if (_enemiesAlive <= 0) RoundManager.Instance.OnRoundComplete();
        }
    }
}