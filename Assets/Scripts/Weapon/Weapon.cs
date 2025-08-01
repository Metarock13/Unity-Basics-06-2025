using UnityEngine;

namespace Scripts
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform barrel;
        [SerializeField] private float damage = 1.0f;
        [SerializeField] private float range = 100.0f;
        [SerializeField] private float force = 3.0f;
        [SerializeField] private float shotDelay = 0.1f;
        [SerializeField] private int magazineSize = 6;
        [SerializeField] private int totalAmmo = 18; 
        
        private float _lastShootTime;
        private int _currentAmmo;
        
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
                    Debug.Log("Did hit");
                }

                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(barrel.forward * force, ForceMode.Impulse);
                }
            }
            
            _currentAmmo--;
            Debug.Log($"Shot! Ammo left: {_currentAmmo}/{magazineSize}");

            _lastShootTime = 0.0f;
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
