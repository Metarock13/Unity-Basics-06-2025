using System;
using UnityEngine;

namespace Code.Gameplay.Features.Character.ComponentDriven
{
    public class CharacterHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;

        public float Health { get; set; }
        public float MaxHealth => maxHealth;
        private bool IsDead => Health <= 0f;

        public event Action<float, float> OnHealthChanged;
        public event Action OnDied;

        private void Construct()
        {
            Health = maxHealth;
            OnHealthChanged?.Invoke(Health, MaxHealth);
        }

        private void Awake()
        {
            if (Health <= 0f) Construct();
        }

        public void TakeDamage(float damage) => TakeDamage(damage, Vector3.zero);

        private void TakeDamage(float damage, Vector3 hitPoint)
        {
            if (IsDead || damage <= 0f) return;

            Health = Mathf.Max(0f, Health - damage);
            OnHealthChanged?.Invoke(Health, MaxHealth);

            if (IsDead)
                OnDied?.Invoke();
        }

        public void Heal(float amount)
        {
            if (amount <= 0f || IsDead) return;
            Health = Mathf.Min(MaxHealth, Health + amount);
            OnHealthChanged?.Invoke(Health, MaxHealth);
        }

        public void FullRestore()
        {
            Health = MaxHealth;
            OnHealthChanged?.Invoke(Health, MaxHealth);
        }
    }
}