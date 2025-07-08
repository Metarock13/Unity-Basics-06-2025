using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class Enemy : MonoBehaviour
    {
        private float _health;

        void Start()
        {
            Debug.LogWarning($"Текущее здоровье врага: {_health} HP");
        }

        public void SetHealth(float initialHealth)
        {
            _health = initialHealth;
            Debug.LogWarning($"У врага установлено здоровье: {_health} HP");
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            Debug.LogWarning($"Игрок нанес врагу (float): {damage}, у врага осталось здоровья: {_health} HP");
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            Debug.LogWarning($"Игрок нанес врагу (int): {damage}, у врага осталось здоровья: {_health} HP");
        }
    }
}