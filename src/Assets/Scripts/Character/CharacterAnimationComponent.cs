using Catalogue;
using UnityEngine;
using UnityEngine.AI;
using Widget;

namespace Character
{
    public class CharacterAnimationComponent : AnimationComponent
    {
        private readonly int _weaponMultiplier = Animator.StringToHash("WeaponMultiplier");
        private readonly int _characterMultiplier = Animator.StringToHash("CharacterMultiplier");

        private readonly NavMeshAgent _navMeshAgent;
        
        public CharacterAnimationComponent(NavMeshAgent navMeshAgent, Animator animator) : base(animator)
        {
            _navMeshAgent = navMeshAgent;
        }

        public void SetWeaponSettings(WeaponSettings weaponSettings)
        {
            Animator.SetFloat(_weaponMultiplier, weaponSettings.WeaponMultiplier);
            Animator.SetFloat(_characterMultiplier, weaponSettings.CharacterMultiplier);
        }
        
        protected override float GetSpeed()
        {
            return Vector3.Project(_navMeshAgent.desiredVelocity, _navMeshAgent.transform.forward).magnitude;
        }
    }
}