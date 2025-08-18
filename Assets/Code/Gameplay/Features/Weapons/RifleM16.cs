using Code.Gameplay.Infrastructure.Services;
using UnityEngine;

namespace Code.Gameplay.Features.Weapons
{
    public class RifleM16 : MonoBehaviour
    {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private StandaloneInputService inputService;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip shotSound;
        [Range(0f, 1f)] [SerializeField] private float shotVolume = 1f;
        [SerializeField] private Vector2 shotPitchRand = new Vector2(0.98f, 1.02f);

        private void Update()
        {
            if (inputService.HasShootInput())
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            var bulletInstance = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
            bulletInstance.transform.position = shootPoint.position;
            bulletInstance.transform.forward = transform.forward;
            bulletInstance.SetDirection(transform.forward, bulletSpeed);

            if (shotSound && SFXPlayer.Instance)
            {
                float pitch = Random.Range(shotPitchRand.x, shotPitchRand.y);
                SFXPlayer.Instance.PlayAt(shotSound, shootPoint.position, shotVolume, pitch,
                    priority: 32, bus: SFXBus.Gunshot);
            }

        }
    }
}