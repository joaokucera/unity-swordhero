using UnityEngine;

namespace Widget
{
    public enum AnimationState
    {
        Attack = 0,
        Move = 1,
        Death = 2
    }

    public abstract class AnimationComponent
    {
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private readonly int _stateHash = Animator.StringToHash("State");
        private readonly int _damageHash = Animator.StringToHash("Damage");
        
        private AnimationState _currentState;
        protected readonly Animator Animator;

        protected abstract float GetSpeed();

        protected AnimationComponent(Animator animator)
        {
            Animator = animator;
        }

        public void UpdateState(bool isMoving)
        {
            if (_currentState == AnimationState.Death)
            {
                return;
            }
            
            SetAnimationState(isMoving ? AnimationState.Move : AnimationState.Attack);
            SetAnimationSpeed();
        }

        public void SetAnimationDamage(int currentHealth, bool isDead)
        {
            Animator.SetTrigger(_damageHash);
            
            if (isDead)
            {
                SetAnimationState(AnimationState.Death);
            }
        }

        private void SetAnimationState(AnimationState nexState)
        {
            _currentState = nexState;
            Animator.SetInteger(_stateHash, (int)_currentState);
        }

        private void SetAnimationSpeed()
        {
            Animator.SetFloat(_speedHash, GetSpeed());
        }
    }
}