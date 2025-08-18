using UnityEngine;
using Code.Gameplay.Features.Enemy;

namespace Code.Gameplay.Features.Weapons
{
    public class BulletDamage : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private bool destroyOnHit = true;

        void OnTriggerEnter(Collider other) => TryHit(other, other.ClosestPoint(transform.position));
        void OnCollisionEnter(Collision c)   => TryHit(c.collider, c.contacts.Length > 0 ? c.contacts[0].point : transform.position);

        private void TryHit(Collider col, Vector3 hitPoint)
        {
            var enemy = col.GetComponentInParent<EnemyHealth>();
            if (enemy)
            {
                enemy.TakeDamage(damage, hitPoint);
            }

            if (destroyOnHit) Destroy(gameObject);
        }
    }
}