using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class HitscanWeapon : WeaponBase
    {
        [Header("Bullet Hole")]
        [SerializeField] private GameObject bulletHolePrefab;
        [SerializeField] private float holeSize = 0.12f;
        [SerializeField] private float normalOffset = 0.002f;
        [SerializeField] private int maxHoles = 60;
        [SerializeField] private float holeLifetime = 20f;

        private readonly Queue<GameObject> _holes = new();
        
        protected override void OnFire()
        {
            if (!barrel) {Debug.LogWarning("No barrel!");
                return; 
            }

            if (Physics.Raycast(barrel.position, barrel.forward, out var hit, range, ~0,
                    QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.TryGetComponent(out HealthController h))
                {
                    h.TakeDamage(damage);
                }

                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(barrel.forward * force, ForceMode.Impulse);
                }

                SpawnBulletHole(hit);
            }
        }

        private void SpawnBulletHole(RaycastHit hit)
        {
            if (!bulletHolePrefab) return;
            
            var pos = hit.point + hit.normal * normalOffset;
            var rot = Quaternion.LookRotation(-hit.normal, Vector3.up);
            rot *= Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            var hole = Instantiate(bulletHolePrefab, pos, rot);
            float s = holeSize * Random.Range(0.95f, 1.05f);
            hole.transform.localScale = new Vector3(s, s, s);
            hole.transform.SetParent(hit.collider.transform, true);
            
            _holes.Enqueue(hole);
            if (_holes.Count > maxHoles)
            {
                var old = _holes.Dequeue();
                if (old) Destroy(old);
            }
            
            if (holeLifetime > 0f) Destroy(hole, holeLifetime);
        }
    }
}