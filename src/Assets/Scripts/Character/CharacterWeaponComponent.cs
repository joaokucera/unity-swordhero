using System;
using Catalogue;
using UnityEngine;
using Weapon;
using Widget;

namespace Character
{
    public class CharacterWeaponComponent
    {
        public event Action<WeaponSettings> OnWeaponChanged = delegate { };

        private readonly IPoolService _poolService;
        private readonly IWeaponService _weaponService;
        private readonly Transform _weaponSlotTransform;

        private WeaponSettings _previousWeaponSettings;
        private WeaponSettings _currentWeaponSettings;
        private WeaponController _currentWeaponController;

        public CharacterWeaponComponent(Transform weaponSlotTransform)
        {
            _poolService = LocatorService.Instance.Get<IPoolService>();
            _weaponService = LocatorService.Instance.Get<IWeaponService>();
            
            _weaponSlotTransform = weaponSlotTransform;

            SetRandomWeapon();
        }

        public void SetRandomWeapon()
        {
            _previousWeaponSettings = _currentWeaponSettings;

            if (_weaponService.TryGetRandomWeaponSettings(out _currentWeaponSettings) &&
                (_previousWeaponSettings is null ||
                 _previousWeaponSettings.WeaponAssetReference.AssetGUID !=
                 _currentWeaponSettings.WeaponAssetReference.AssetGUID))
            {
                AddressableHelper.LoadAsync(_currentWeaponSettings.WeaponAssetReference, OnGetWeaponPrefab);
            }
        }

        private void OnGetWeaponPrefab(GameObject weaponPrefab)
        {
            if (_currentWeaponController != null)
            {
                _poolService.ReturnPooledObject(_currentWeaponController.gameObject);
            }

            var weaponInstance = _poolService.GetPooledObject(weaponPrefab);
            weaponInstance.transform.SetParent(_weaponSlotTransform, false);

            _currentWeaponController = weaponInstance.GetComponent<WeaponController>();
            _currentWeaponController.SetData(_currentWeaponSettings);

            OnWeaponChanged(_currentWeaponSettings);
        }

        public void UpdateWeaponDamage(bool isMoving)
        {
            if (_currentWeaponController != null)
            {
                _currentWeaponController.UpdateData(!isMoving);
            }
        }
    }
}