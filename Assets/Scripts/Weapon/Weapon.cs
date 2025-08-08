using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class Weapon : MonoBehaviour
    {
        [Header("Weapon settings")]
        [SerializeField] private Transform barrel;
        [SerializeField] private float damage = 1.0f;
        [SerializeField] private float range = 100.0f;
        [SerializeField] private float force = 3.0f;
        [SerializeField] private float shotDelay = 0.1f;
        [SerializeField] private int magazineSize = 6;
        [SerializeField] private int totalAmmo = 18; 
        
        [Header("Bullet hole")]
        [SerializeField] private GameObject bulletHolePrefab;
        [SerializeField] private float holeSize = 0.12f;
        [SerializeField] private float normalOffset = 0.002f;
        [SerializeField] private int maxHoles = 60;
        [SerializeField] private float holeLifetime = 20f;
        
        private float _lastShootTime;
        private int _currentAmmo;
        private readonly Queue<GameObject> _holes = new();
        
        private void Start()
        {
            _currentAmmo = magazineSize;
        }

        private void Update()
        {
            if (_lastShootTime < shotDelay)
            {
                _lastShootTime += Time.deltaTime;
            }
        }

        public void Fire()
        {
            if (_lastShootTime < shotDelay) return;
            if (_currentAmmo <= 0)
            {
                Debug.Log("No ammo, please recharge!");
                return;
            }

            if (Physics.Raycast(barrel.position, barrel.forward, out RaycastHit hit, range))
            {
                if (hit.collider.TryGetComponent(out HealthController health))
                {
                    health.TakeDamage(damage);
                }

                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(barrel.forward * force, ForceMode.Impulse);
                }

                SpawnBulletHole(hit);
            }
            
            _currentAmmo--;
            Debug.Log($"Shot! Ammo left: {_currentAmmo}/{magazineSize}");
            _lastShootTime = 0.0f;
        }

        private void SpawnBulletHole(RaycastHit hit)
        {
            if (!bulletHolePrefab) return;
            
            Vector3 pos = hit.point + hit.normal * normalOffset;
            
            Quaternion rot = Quaternion.LookRotation(-hit.normal, Vector3.up);
            rot *= Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            GameObject hole = Instantiate(bulletHolePrefab, pos, rot);
            
            float s = holeSize * Random.Range(0.95f, 1.05f);
            hole.transform.localScale = new Vector3(s, s, s);
            
            hole.transform.SetParent(hit.collider.transform, true);
            
            _holes.Enqueue(hole);
            if (_holes.Count > maxHoles)
            {
                var old = _holes.Dequeue();
                if (old) Destroy(old);
            }

            if (holeLifetime > 0f)
            {
                Destroy(hole, holeLifetime);
            }
        }
        
        public void Recharge()
        {
            if (_currentAmmo == magazineSize)
            {
                Debug.Log("Magazine is full");
                return;
            }

            int ammoNeeded = magazineSize - _currentAmmo;
            int ammoToLoad = Mathf.Min(ammoNeeded, totalAmmo);

            _currentAmmo += ammoToLoad;
            totalAmmo -= ammoToLoad;

            Debug.Log($"Reloaded. Ammo: {_currentAmmo}/{magazineSize}. Total ammo left: {totalAmmo}");
        }
    }
}
