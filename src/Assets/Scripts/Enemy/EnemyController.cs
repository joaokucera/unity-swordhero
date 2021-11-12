using Catalogue;
using UnityEngine;
using UnityEngine.UI;
using Weapon;
using Widget;

namespace Enemy
{
    public class EnemyController : MonoBehaviour, IDamageComponent
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Image healthBarImage;
        
        private EnemySettings _enemySettings;
        private EnemyAnimationComponent _animationComponent;
        private EnemyMovementComponent _movementComponent;
        private HealthComponent _healthComponent;
        private Vector3 _previousPosition;
        
        public void SetData(EnemySettings enemySettings)
        {
            _enemySettings = enemySettings;
            
            _animationComponent = new EnemyAnimationComponent(enemySettings, animator);
            _movementComponent = new EnemyMovementComponent(enemySettings, transform);
            _healthComponent = new HealthComponent(healthBarImage, enemySettings.EnemyHealth);

            RegisterEvents();
        }
        
        private void RegisterEvents()
        {
            _healthComponent.OnHealthChanged += _animationComponent.SetAnimationDamage;
            _healthComponent.OnHealthChanged += CheckHealthState;
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void UnregisterEvents()
        {
            _healthComponent.OnHealthChanged -= _animationComponent.SetAnimationDamage;
            _healthComponent.OnHealthChanged -= CheckHealthState;
        }
        
        private void CheckHealthState(int currentHealth, bool isDead)
        {
            if (!isDead)
            {
                return;
            }
            
            UnregisterEvents();
        }
        
        private void OnTriggerEnter(Collider otherCollider)
        {
            if (_healthComponent.IsDead)
            {
                return;
            }
            
            const string weaponTag = "Weapon";

            if (otherCollider.CompareTag(weaponTag) &&
                otherCollider.TryGetComponent(out IDamageComponent damageComponent) &&
                damageComponent.CanApplyDamage())
            {
                _healthComponent.SetDamage(damageComponent.GetDamageToApply());
            }
        }

        public bool CanApplyDamage()
        {
            return !_healthComponent.IsDead;
        }

        public int GetDamageToApply()
        {
            return _enemySettings.EnemyDamage;
        }

        private void Update()
        {
            if (_healthComponent.IsDead)
            {
                return;
            }
            
            Vector3 currentPosition = transform.transform.position;
            bool isMoving = Vector3.Distance(_previousPosition, currentPosition) > 0.1f;
            
            _animationComponent.UpdateState(isMoving);
            _movementComponent.UpdateLootAt(isMoving);
            _movementComponent.UpdateMovement(isMoving);

            _previousPosition = currentPosition;
        }
    }
}