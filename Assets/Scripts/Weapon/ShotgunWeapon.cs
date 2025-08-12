using UnityEngine;
using Configs;

namespace Scripts
{
    public class ShotgunWeapon : HitscanWeapon
    {
        [Header("Shotgun")] 
        [SerializeField] private int pellets = 8;
        [SerializeField] private float spreadDegrees = 3f;

        protected override void Awake()
        {
            base.Awake();
            if (config is ShotgunWeaponConfig sc)
            {
                pellets = sc.Pellets;
                spreadDegrees = sc.SpreadDegrees;
            }
        }

        protected override void OnFire()
        {
            if (!barrel) return;

            for (int i = 0; i < pellets; i++)
            {
                var dir = Quaternion.Euler(
                    Random.Range(-spreadDegrees, spreadDegrees),
                    Random.Range(-spreadDegrees, spreadDegrees),
                    0f) * barrel.forward;

                if (Physics.Raycast(barrel.position, dir, out var hit, range, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.TryGetComponent(out HealthController h))
                    {
                        h.TakeDamage(damage);
                    }

                    if (hit.rigidbody)
                    {
                        hit.rigidbody.AddForce(dir * force / pellets, ForceMode.Impulse);
                    }
                    
                    SpawnBulletHole(hit);
                }
            }
        }
    }
}