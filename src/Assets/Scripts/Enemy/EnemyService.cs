using System.Collections;
using System.Collections.Generic;
using Catalogue;
using UnityEngine;
using Widget;

namespace Enemy
{
    public interface IEnemyService : IGameService
    {
        bool TryGetNearestEnemy(Vector3 origin, out EnemyController target);
        void DespawnAllEnemies();
    }

    public class EnemyService : MonoBehaviour, IEnemyService
    {
        private IPoolService _poolService;
        private EnemyCatalogue _enemyCatalogue;
        private readonly List<Coroutine> _spawnCoroutines = new List<Coroutine>();
        private readonly List<EnemyController> _enemyController = new List<EnemyController>();

        private IPoolService PoolService => _poolService ??= LocatorService.Instance.Get<IPoolService>();

        private void Awake()
        {
            // TODO: It would be removed by installing scriptable objects with zenject's scene context 
            _enemyCatalogue = Resources.Load<EnemyCatalogue>("EnemyCatalogue");
        }

        private void OnEnable()
        {
            foreach (EnemySettings enemySpawnerSettings in _enemyCatalogue.GetEnemySettings())
            {
                WaitForSeconds waitForSeconds = new WaitForSeconds(enemySpawnerSettings.EnemySpawnIntervalTime);
                Coroutine coroutine = StartCoroutine(SpawnEnemy(enemySpawnerSettings, waitForSeconds));

                _spawnCoroutines.Add(coroutine);
            }
        }

        private void OnDisable()
        {
            foreach (Coroutine coroutine in _spawnCoroutines)
            {
                StopCoroutine(coroutine);
            }

            _spawnCoroutines.Clear();
        }

        private IEnumerator SpawnEnemy(EnemySettings enemySettings, WaitForSeconds waitForSeconds)
        {
            while (true)
            {
                AddressableHelper.LoadAsync(enemySettings.EnemyAssetReference,
                    prefab => OnGetEnemyPrefab(prefab, enemySettings));
                yield return waitForSeconds;
            }
        }

        private void OnGetEnemyPrefab(GameObject enemyPrefab, EnemySettings enemySettings)
        {
            GameObject enemyObject = PoolService.GetPooledObject(enemyPrefab);
            enemyObject.transform.SetParent(transform, false);
            enemyObject.transform.localPosition =
                NavMeshHelper.GetRandomLocation(transform, enemySettings.EnemyLocationRadius);

            EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
            enemyController.SetData(enemySettings);

            _enemyController.Add(enemyController);
        }

        public bool TryGetNearestEnemy(Vector3 origin, out EnemyController enemyTarget)
        {
            EnemyController nearestEnemy = null;
            float smallestDistance = float.MaxValue;

            foreach (EnemyController enemyController in _enemyController)
            {
                if (!enemyController.CanApplyDamage())
                {
                    continue;
                }

                float distance = (enemyController.transform.position - origin).sqrMagnitude;
                if (distance < smallestDistance)
                {
                    nearestEnemy = enemyController;
                    smallestDistance = distance;
                }
            }

            if (nearestEnemy != null)
            {
                enemyTarget = nearestEnemy;
                return true;
            }

            enemyTarget = null;
            return false;
        }

        public void DespawnAllEnemies()
        {
            _poolService.ReturnAllPooledChildren(gameObject);
            _enemyController.Clear();
        }
    }
}