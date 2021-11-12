using System;
using UnityEngine.UI;

namespace Widget
{
    public class HealthComponent
    {
        public event Action<int, bool> OnHealthChanged = delegate { };
    
        private readonly Image _healthBarImage;
        private readonly int _maxHealth;
        private int _currentHealth;
        
        public bool IsDead => _currentHealth <= 0;

        public HealthComponent(Image healthBarImage, int maxHealth)
        {
            _healthBarImage = healthBarImage;
            _maxHealth = maxHealth;

            SetHeal(_maxHealth);    
        }

        public void SetDamage(int damage)
        {
            _currentHealth -= damage;
        
            TriggerCurrentHealthChangedEvent();
        }
    
        public void SetHeal(int heal)
        {
            _currentHealth += heal;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }

            TriggerCurrentHealthChangedEvent();
        }

        private void TriggerCurrentHealthChangedEvent()
        {
            _healthBarImage.fillAmount = _currentHealth * 1f / _maxHealth;
            
            OnHealthChanged(_currentHealth, IsDead);
        }
    }
}