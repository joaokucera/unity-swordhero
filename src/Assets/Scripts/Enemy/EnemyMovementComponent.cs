using Catalogue;
using Character;
using UnityEngine;
using Widget;
using CharacterController = Character.CharacterController;

namespace Enemy
{
    public class EnemyMovementComponent
    {
        private readonly ICharacterService _characterService;
        private readonly EnemySettings _enemySettings;
        private readonly Transform _enemyTransform;

        private Vector3 _nextLocation;
        private float _timeLeftToNextLocation;

        public EnemyMovementComponent(EnemySettings enemySettings, Transform enemyTransform)
        {
            _characterService = LocatorService.Instance.Get<ICharacterService>();

            _enemySettings = enemySettings;
            _enemyTransform = enemyTransform;
        }

        public void UpdateLootAt(bool isMoving)
        {
            if (isMoving)
            {
                return;
            }

            if (_characterService.TryGetCharacterController(out CharacterController characterController))
            {
                _enemyTransform.LookAt(characterController.transform);
            }
        }
        
        public void UpdateMovement(bool isMoving)
        {
            _timeLeftToNextLocation -= Time.deltaTime;

            if (_timeLeftToNextLocation <= 0)
            {
                _nextLocation = NavMeshHelper.GetRandomLocation(_enemyTransform, _enemySettings.EnemyLocationRadius);
                _timeLeftToNextLocation += Random.Range(_enemySettings.EnemyMovementMinIntervalTime,
                    _enemySettings.EnemyMovementMaxIntervalTime);
            }

            Vector3 newPosition =
                Vector3.Lerp(_enemyTransform.position, _nextLocation, _enemySettings.EnemyMovementSpeed);
            _enemyTransform.transform.position = newPosition;
        }
    }
}