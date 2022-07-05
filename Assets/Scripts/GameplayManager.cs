using System;

namespace AFSInterview
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public enum TowersType
    {
        towerNormal,
        towerBurst
    }

    [System.Serializable]
    public class Towers
    {
        public TowersType towerType;
        public GameObject towerPrefab;
    }

    public class GameplayManager : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject enemyPrefab;

        [Header("Settings")] [SerializeField] private Vector2 boundsMin;
        [SerializeField] private Vector2 boundsMax;
        [SerializeField] private float enemySpawnRate;
        [SerializeField] private LayerMask towerSpawnLayer;


        [Header("UI")] [SerializeField] private TextMeshProUGUI enemiesCountText;
        [SerializeField] private TextMeshProUGUI scoreText;

        [Header("Towers List")] [SerializeField]
        private List<Towers> towersList = new List<Towers>();

        private List<Enemy> enemies;
        private float enemySpawnTimer;
        private int score;

        private void Awake()
        {
            enemies = new List<Enemy>();
        }

        private void Start()
        {
            SetEnemyCounter();
            SetScoreCounter();
        }

        private void Update()
        {
            enemySpawnTimer -= Time.deltaTime;

            if (enemySpawnTimer <= 0f)
            {
                SpawnEnemy();
                enemySpawnTimer = enemySpawnRate;
            }
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Camera.main == null) return;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, towerSpawnLayer))
                {
                    var spawnPosition = hit.point;
                    SpawnTower(spawnPosition, TowersType.towerNormal);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (Camera.main == null) return;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, towerSpawnLayer))
                {
                    var spawnPosition = hit.point;
                    SpawnTower(spawnPosition, TowersType.towerBurst);
                }
            }
        }

        private void SpawnEnemy()
        {
            var position = new Vector3(Random.Range(boundsMin.x, boundsMax.x), enemyPrefab.transform.position.y,
                Random.Range(boundsMin.y, boundsMax.y));

            var enemy = Instantiate(enemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
            enemy.OnEnemyDied += Enemy_OnEnemyDied;
            enemy.Initialize(boundsMin, boundsMax);

            enemies.Add(enemy);
            SetEnemyCounter();
        }


        private void Enemy_OnEnemyDied(Enemy enemy)
        {
            enemies.Remove(enemy);
            AddScore();
            SetEnemyCounter();
            SetScoreCounter();
        }

        private void SpawnTower(Vector3 position, TowersType towerType)
        {
            var towerToSpawn = towersList.Find(x => x.towerType == towerType).towerPrefab;
            var spawnPosition = position;
            spawnPosition.y = towerToSpawn.transform.position.y;
            var tower = Instantiate(towerToSpawn, spawnPosition, Quaternion.identity);
            switch (towerType)
            {
                case TowersType.towerNormal:
                    tower.GetComponent<SimpleTower>().Initialize(enemies);
                    break;
                case TowersType.towerBurst:
                    tower.GetComponent<BurstTower>().Initialize(enemies);
                    break;
            }
        }

        private void AddScore()
        {
            score++;
        }

        private void SetScoreCounter()
        {
            scoreText.text = "Score: " + score;
        }

        private void SetEnemyCounter()
        {
            enemiesCountText.text = "Enemies: " + enemies.Count;
        }
    }
}