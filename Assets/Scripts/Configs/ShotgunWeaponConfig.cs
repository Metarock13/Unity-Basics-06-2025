using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ShotgunWeaponConfig", menuName = "Configs/Shotgun Weapon Config")]
    public class ShotgunWeaponConfig : HitscanWeaponConfig
    {
        [Header("Shotgun Settings")] 
        [SerializeField] private int pellets = 8;
        [SerializeField] private float spreadDegrees = 3f;

        public int Pellets => pellets;
        public float SpreadDegrees => spreadDegrees;
    }
}