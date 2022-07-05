using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AFSInterview
{
    public class BurstTower : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private float initialVelocity = 10f;
        [SerializeField] private float shootFireRate = 0.25f;

        private Enemy targetEnemy;

        private IReadOnlyList<Enemy> enemies;

        public void Initialize(IReadOnlyList<Enemy> enemies)
        {
            this.enemies = enemies;
            DrawEnemyAndShoot();
        }

        private void Update()
        {
            if (targetEnemy != null)
            {
                var lookRotation = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y,
                    transform.rotation.eulerAngles.z);
            }
        }

        private void DrawEnemyAndShoot()
        {
            StartCoroutine(DrawEnemyAndShootIe());
        }

        private IEnumerator DrawEnemyAndShootIe()
        {
            yield return new WaitForSeconds(5f);
            if (enemies.Any() && enemies != null)
            {
                var index = Random.Range(0, enemies.Count - 1);
                if (enemies[index].transform != null)
                {
                    targetEnemy = enemies[index];
                    for (int i = 0; i < 3; i++)
                    {
                        if (targetEnemy != null)
                            StartCoroutine(EnemyShootIe(targetEnemy.transform));
                        yield return new WaitForSeconds(shootFireRate);
                    }
                }
            }

            StartCoroutine(DrawEnemyAndShootIe());
        }


        private Vector3 enemyPredictPos;

        private IEnumerator EnemyShootIe(Transform enemy)
        {
            var enemyLastPos = Vector3.zero;
            enemyPredictPos = Vector3.zero;
            var enemyVelocity = Vector3.zero;
            if (enemy != null)
            {
                enemyLastPos = enemy.position;
                yield return new WaitForSeconds(0.001f);
                enemyVelocity = (enemy.position - enemyLastPos) / 0.001f;

                enemyPredictPos =
                    enemyVelocity * 0.15f + (enemy.position); //(0.3f*shootFireRate*enemyVelocity +enemyDir);

                CalcShootForceAndShootBall(enemyPredictPos, initialVelocity);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(enemyPredictPos, 0.24f);
        }

        private void CalcShootForceAndShootBall(Vector3 targetPredictPos, float initialVelocity)
        {
            var direction = (targetPredictPos - bulletSpawnPoint.position).normalized;
            var distance =
                Vector3.Distance(targetPredictPos, bulletSpawnPoint.position);

            var shootForce = direction * 250 * distance;
            ShootBall(shootForce);
        }

        private void ShootBall(Vector3 shootForce)
        {
            var ball = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity)
                .GetComponent<Rigidbody>();
            ball.AddForce(shootForce);
        }
    }
}