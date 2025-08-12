using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "HitscanWeaponConfig", menuName = "Configs/Hitscan Weapon Config")]
    public class HitscanWeaponConfig : WeaponConfigBase
    {
        [Header("Bullet Hole Settings")]
        [SerializeField] private GameObject bulletHolePrefab;
        [SerializeField] private float holeSize = 0.12f;
        [SerializeField] private float normalOffset = 0.002f;
        [SerializeField] private int maxHoles = 60;
        [SerializeField] private float holeLifetime = 20f;
        
        public GameObject BulletHolePrefab => bulletHolePrefab;
        public float HoleSize => holeSize;
        public float NormalOffset => normalOffset;
        public int MaxHoles => maxHoles;
        public float HoleLifetime => holeLifetime;
    }
}