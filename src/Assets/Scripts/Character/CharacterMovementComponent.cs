using Enemy;
using UnityEngine;
using UnityEngine.AI;
using Widget;

namespace Character
{
    public class CharacterMovementComponent
    {
        private readonly IEnemyService _enemyService;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _characterTransform;

        private EnemyController _currentEnemyTarget;

        public CharacterMovementComponent(NavMeshAgent navMeshAgent, Transform characterTransform)
        {
            _enemyService = LocatorService.Instance.Get<IEnemyService>();
            
            _navMeshAgent = navMeshAgent;
            _characterTransform = characterTransform;
        }

        public void StopMovement()
        {
            _navMeshAgent.isStopped = false;
        }
        
        public void UpdateMovement(RaycastHit raycastHit)
        {
            _navMeshAgent.SetDestination(raycastHit.point);
        }

        public void UpdateLootAt(bool isMoving)
        {
            if (isMoving)
            {
                _currentEnemyTarget = null;
                return;
            }

            if (_currentEnemyTarget != null && _currentEnemyTarget.CanApplyDamage() ||
                _enemyService.TryGetNearestEnemy(_characterTransform.position, out _currentEnemyTarget))
            {
                _characterTransform.LookAt(_currentEnemyTarget.transform);
            }
        }
    }
}