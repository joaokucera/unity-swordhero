using Catalogue;
using UnityEngine;
using Widget;

namespace Enemy
{
    public class EnemyAnimationComponent : AnimationComponent
    {
        private readonly EnemySettings _enemySettings;
        
        public EnemyAnimationComponent(EnemySettings enemySettings, Animator animator) : base(animator)
        {
            _enemySettings = enemySettings;
        }
        
        protected override float GetSpeed()
        {
            return _enemySettings.EnemyMovementSpeed;
        }
    }
}