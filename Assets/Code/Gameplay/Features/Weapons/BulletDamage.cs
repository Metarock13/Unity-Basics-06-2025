using UnityEngine;
using Code.Gameplay.Features.Enemy;

namespace Code.Gameplay.Features.Weapons
{
    public class BulletDamage : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private bool destroyOnHit = true;

        private void OnTriggerEnter(Collider other)
        {
            TryHit(other, transform.position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Vector3 hitPoint = collision.contacts.Length > 0 
                ? collision.contacts[0].point 
                : transform.position;

            TryHit(collision.collider, hitPoint);
        }

        private void TryHit(Collider col, Vector3 hitPoint)
        {
            var enemy = col.GetComponentInParent<EnemyHealth>();
            if (enemy)
            {
                enemy.TakeDamage(damage, hitPoint);
            }

            if (destroyOnHit)
                Destroy(gameObject);
        }
    }
}