using System.Collections.Generic;
using Configs;
using UnityEngine;

namespace Scripts
{
    public class HitscanWeapon : WeaponBase
    {
        [Header("Bullet Hole")]
        [SerializeField] protected GameObject bulletHolePrefab;
        [SerializeField] protected float holeSize = 0.12f;
        [SerializeField] protected float normalOffset = 0.002f;
        [SerializeField] protected int maxHoles = 60;
        [SerializeField] protected float holeLifetime = 20f;

        private readonly Queue<GameObject> _holes = new();

        protected override void Awake()
        {
            base.Awake();
            if (config is HitscanWeaponConfig hc)
            {
                bulletHolePrefab = hc.BulletHolePrefab;
                holeSize = hc.HoleSize;
                normalOffset = hc.NormalOffset;
                maxHoles = hc.MaxHoles;
                holeLifetime = hc.HoleLifetime;
            }
        }

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

        protected void SpawnBulletHole(RaycastHit hit)
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