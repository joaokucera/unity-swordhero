using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Catalogue
{
    [CreateAssetMenu(fileName = "New WeaponCatalogue", menuName = "Catalogues/WeaponCatalogue")]
    public class WeaponCatalogue : ScriptableObject
    {
        [SerializeField] private WeaponSettings[] weaponSettings;

        public bool TryGetRandomWeaponSettings(out WeaponSettings result)
        {
            if (weaponSettings.Length > 0)
            {
                int random = Random.Range(0, 1000);
                int index = random % weaponSettings.Length;
                
                result = weaponSettings[index];
                return true;
            }

            result = null;
            return false;
        }
    }

    [Serializable]
    public class WeaponSettings
    {
        public AssetReference WeaponAssetReference;
        public int WeaponDamage;
        public float WeaponMultiplier;
        public float CharacterMultiplier;
    }
}