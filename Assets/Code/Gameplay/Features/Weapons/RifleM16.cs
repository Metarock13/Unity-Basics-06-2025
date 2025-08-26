using Code.Gameplay.Infrastructure.Services;
using Code.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Gameplay.Features.Weapons
{
    public class RifleM16 : MonoBehaviour
    {
        [Header("Shoot")]
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float bulletSpeed = 40f;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private StandaloneInputService inputService;

        [Header("Ammo")]
        [SerializeField] private int magazineSize = 30;
        [SerializeField] private int reserve = 90;

        private int inMagazine;
        private HUDController hud;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip shotSound;
        [Range(0f, 1f)] [SerializeField] private float shotVolume = 1f;
        [SerializeField] private Vector2 shotPitchRand = new Vector2(0.98f, 1.02f);

        private void Awake()
        {
            inMagazine = magazineSize;
            hud = FindFirstObjectByType<HUDController>();
        }

        private void Start()
        {
            UpdateHUD();
        }

        private void Update()
        {
            if (PauseMenu.IsPaused) return;
            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;

            if (inputService.HasShootInput())
                Shoot();

            if (Input.GetKeyDown(KeyCode.R))
                Reload();
        }

        private void Shoot()
        {
            if (inMagazine <= 0)
            {
                Debug.Log("Click! No ammo in magazine.");
                return;
            }

            var bulletInstance = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();
            bulletInstance.SetDirection(transform.forward, bulletSpeed);

            inMagazine--;

            if (shotSound && SFXPlayer.Instance)
            {
                float pitch = Random.Range(shotPitchRand.x, shotPitchRand.y);
                SFXPlayer.Instance.PlayAt(shotSound, shootPoint.position, shotVolume, pitch,
                    priority: 32, bus: SFXBus.Gunshot);
            }

            UpdateHUD();
        }

        private void Reload()
        {
            if (reserve <= 0 || inMagazine == magazineSize) return;

            int need = magazineSize - inMagazine;
            int toLoad = Mathf.Min(need, reserve);

            inMagazine += toLoad;
            reserve -= toLoad;

            Debug.Log($"Reloaded: {inMagazine}/{reserve}");

            UpdateHUD();
        }

        private void UpdateHUD()
        {
            if (hud)
                hud.SetAmmo(inMagazine, reserve);
        }
    }
}
