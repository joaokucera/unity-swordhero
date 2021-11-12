using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Weapon;
using Widget;

namespace Character
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform weaponSlotTransform;
        [SerializeField] private Image healthBarImage;

        private IInputService _inputService;
        private IGameStateService _gameStateService;
        private INavigationService _navigationService;
        private CharacterAnimationComponent _animationComponent;
        private CharacterMovementComponent _movementComponent;
        private CharacterWeaponComponent _weaponComponent;
        private HealthComponent _healthComponent;

        public void Awake()
        {
            _inputService = LocatorService.Instance.Get<IInputService>();
            _gameStateService = LocatorService.Instance.Get<IGameStateService>();
            _navigationService = LocatorService.Instance.Get<INavigationService>();
            
            _animationComponent = new CharacterAnimationComponent(navMeshAgent, animator);
            _movementComponent = new CharacterMovementComponent(navMeshAgent, transform);
            _weaponComponent = new CharacterWeaponComponent(weaponSlotTransform);
            
            ICharacterService characterService = LocatorService.Instance.Get<ICharacterService>();
            _healthComponent = new HealthComponent(healthBarImage, characterService.GetCharacterSettings().CharacterMaxHealth);
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _inputService.OnWeaponKeysPressed += _weaponComponent.SetRandomWeapon;
            _navigationService.OnRaycastHit += _movementComponent.UpdateMovement;
            _weaponComponent.OnWeaponChanged += _animationComponent.SetWeaponSettings;
            _healthComponent.OnHealthChanged += _animationComponent.SetAnimationDamage;
            _healthComponent.OnHealthChanged += CheckHealthState;
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void UnregisterEvents()
        {
            _inputService.OnReloadKeyPressed -= _gameStateService.ReloadGame;
            _inputService.OnWeaponKeysPressed -= _weaponComponent.SetRandomWeapon;
            _navigationService.OnRaycastHit -= _movementComponent.UpdateMovement;
            _weaponComponent.OnWeaponChanged -= _animationComponent.SetWeaponSettings;
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
            
            _movementComponent.StopMovement();
            _inputService.OnReloadKeyPressed += _gameStateService.ReloadGame;
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (_healthComponent.IsDead)
            {
                return;
            }
            
            const string enemyTag = "Enemy";

            if (otherCollider.CompareTag(enemyTag) &&
                otherCollider.TryGetComponent(out IDamageComponent damageComponent) &&
                damageComponent.CanApplyDamage())
            {
                _healthComponent.SetDamage(damageComponent.GetDamageToApply());
            }
        }

        private void Update()
        {
            if (_healthComponent.IsDead)
            {
                return;
            }
            
            bool isMoving = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;

            _animationComponent.UpdateState(isMoving);
            _movementComponent.UpdateLootAt(isMoving);
            _weaponComponent.UpdateWeaponDamage(isMoving);
        }
    }
}