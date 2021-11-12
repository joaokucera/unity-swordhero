using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Catalogue
{
    [CreateAssetMenu(fileName = "New EnemySpawnerCatalogue", menuName = "Catalogues/EnemySpawnerCatalogue")]
    public class EnemyCatalogue : ScriptableObject
    {
        [SerializeField] private EnemySettings[] enemySettings;

        public IEnumerable<EnemySettings> GetEnemySettings()
        {
            foreach (EnemySettings spawnerSettings in enemySettings)
            {
                yield return spawnerSettings;
            }
        }
    }

    [Serializable]
    public class EnemySettings
    {
        public AssetReference EnemyAssetReference;
        public int EnemyLocationRadius;
        public int EnemyHealth;
        public int EnemyDamage;
        public float EnemyMovementSpeed;
        public float EnemyMovementMinIntervalTime;
        public float EnemyMovementMaxIntervalTime;
        public float EnemySpawnIntervalTime;
    }
}