using Catalogue;
using UnityEngine;
using Widget;

namespace Weapon
{
    public class WeaponController : MonoBehaviour, IDamageComponent
    {
        [SerializeField] private Collider colliderComponent;
        
        private WeaponSettings _weaponSettings;

        public void SetData(WeaponSettings weaponSettings)
        {
            _weaponSettings = weaponSettings;

            UpdateData(false);
        }

        public void UpdateData(bool canApplyDamage)
        {
            colliderComponent.enabled = canApplyDamage;
        }

        public bool CanApplyDamage()
        {
            return true;
        }

        public int GetDamageToApply()
        {
            return _weaponSettings.WeaponDamage;
        }
    }
}