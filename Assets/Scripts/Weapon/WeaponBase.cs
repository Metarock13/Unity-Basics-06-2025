using Configs;
using UnityEngine;

namespace Scripts
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] protected Transform barrel;
        [SerializeField] protected float damage = 1f;
        [SerializeField] protected float range = 100f;
        [SerializeField] protected float force = 3f;
        [SerializeField] protected float shotDelay = 0.1f;
        [SerializeField] protected int magazineSize = 6;
        [SerializeField] protected int totalAmmo = 18;
        [SerializeField] protected WeaponConfigBase config;

        protected float _cooldown;
        protected float _currentAmmo;

        public virtual void OnSelect() => gameObject.SetActive(true);
        public virtual void OnDeselect() => gameObject.SetActive(false);

        protected virtual void Awake()
        {
            if (config)
            {
                damage = config.Damage;
                range = config.Range;
                force = config.Force;
                shotDelay = config.ShotDelay;
                magazineSize = config.MagazineSize;
                totalAmmo = config.TotalAmmo;
            }
            _currentAmmo = magazineSize;
        }

        protected virtual void Update()
        {
            if (_cooldown < shotDelay) _cooldown += Time.deltaTime;
        }

        public void Fire()
        {
            if (_cooldown < shotDelay) return;

            if (_currentAmmo <= 0)
            {
                Debug.Log("No ammo, please recharge!");
                return;
            }

            _cooldown = 0f;
            _currentAmmo--;
            OnFire();
        }

        public virtual void Recharge()
        {
            if (Mathf.Approximately(_currentAmmo, magazineSize))
            {
                Debug.Log("Magazine is full");
                return;
            }
            
            int need = (int)(magazineSize - _currentAmmo);
            int toLoad = Mathf.Min(need, totalAmmo);
            _currentAmmo += toLoad;
            totalAmmo -= toLoad;
            
            Debug.Log($"Reloaded. Ammo: {_currentAmmo}/{magazineSize}. Total left: {totalAmmo}");
        }
        
        protected abstract void OnFire();
    }
}