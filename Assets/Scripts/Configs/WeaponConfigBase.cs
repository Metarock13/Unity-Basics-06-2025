using UnityEngine;

namespace Configs
{
    public abstract class WeaponConfigBase : ScriptableObject
    {
        [Header("Common Weapon Settings")]
        [SerializeField] private float damage = 1f;
        [SerializeField] private float range = 100f;
        [SerializeField] private float force = 3f;
        [SerializeField] private float shotDelay = 0.1f;
        [SerializeField] private int magazineSize = 6;
        [SerializeField] private int totalAmmo = 18;
        
        public float Damage => damage;
        public float Range => range;
        public float Force => force;
        public float ShotDelay => shotDelay;
        public int MagazineSize => magazineSize;
        public int TotalAmmo => totalAmmo;
    }
}