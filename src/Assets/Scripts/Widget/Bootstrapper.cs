using Character;
using Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapon;

namespace Widget
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            LocatorService.Init();

            ICameraService cameraService = CreateCameraService();
            IInputService inputService = CreateInputService();
            INavigationService navigationService = CreateNavigationService(cameraService, inputService);
            ICharacterService characterService = CreateCharacterService();
            IWeaponService weaponService = CreateWeaponService();
            IPoolService poolService = CreatePoolService();
            IEnemyService enemyService = CreateEnemyService();
            IGameStateService gameStateService = CreateGameStateService(enemyService);
            
            LocatorService.Instance.Register(cameraService);
            LocatorService.Instance.Register(inputService);
            LocatorService.Instance.Register(navigationService);
            LocatorService.Instance.Register(characterService);
            LocatorService.Instance.Register(weaponService);
            LocatorService.Instance.Register(poolService);
            LocatorService.Instance.Register(enemyService);
            LocatorService.Instance.Register(gameStateService);

            SceneManager.LoadSceneAsync(sceneBuildIndex: 1, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(sceneBuildIndex: 2, LoadSceneMode.Additive);
        }

        private static ICameraService CreateCameraService()
        {
            return new CameraService();
        }

        private static IInputService CreateInputService()
        {
            return GameObjectHelper.CreateGameObject<InputService>("InputService");
        }

        private static INavigationService CreateNavigationService(ICameraService cameraService,
            IInputService inputService)
        {
            return new NavigationService(cameraService, inputService);
        }

        private static ICharacterService CreateCharacterService()
        {
            return new CharacterService();
        }

        private static IWeaponService CreateWeaponService()
        {
            return new WeaponService();
        }

        private static IPoolService CreatePoolService()
        {
            return GameObjectHelper.CreateGameObject<PoolService>("PoolService");
        }

        private static IEnemyService CreateEnemyService()
        {
            return GameObjectHelper.CreateGameObject<EnemyService>("EnemyService");
        }
        
        private static IGameStateService CreateGameStateService(IEnemyService enemyService)
        {
            return new GameStateService(enemyService);
        }
    }
}