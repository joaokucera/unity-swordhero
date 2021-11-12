using Catalogue;
using UnityEngine;
using Widget;

namespace Weapon
{
    public interface IWeaponService : IGameService
    {
        bool TryGetRandomWeaponSettings(out WeaponSettings weaponSettings);
    }

    public class WeaponService : IWeaponService
    {
        private readonly WeaponCatalogue _weaponService;

        public WeaponService()
        {
            // TODO: It would be removed by installing scriptable objects with zenject's scene context
            _weaponService = Resources.Load<WeaponCatalogue>("WeaponCatalogue");
        }

        public bool TryGetRandomWeaponSettings(out WeaponSettings weaponSettings)
        {
            return _weaponService.TryGetRandomWeaponSettings(out weaponSettings);
        }
    }
}