using UnityEngine;

namespace Code.Gameplay.Features.Character.ComponentDriven
{
    public class CharacterHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;

        public float Health => maxHealth;
        public float MaxHealth => maxHealth;
        public bool IsDead => false;

        public void Construct() { }

        public void TakeDamage(float damage) { }
        public void TakeDamage(float damage, Vector3 hitPoint) { }
    }
}