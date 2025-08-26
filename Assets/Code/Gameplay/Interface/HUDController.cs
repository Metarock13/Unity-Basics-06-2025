using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HUDController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Code.Gameplay.Features.Character.ComponentDriven.CharacterHealth health;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image healthFill;

        [Header("Ammo")]
        [SerializeField] private TextMeshProUGUI ammoText;

        private int _inMag = 30;
        private int _reserve = 90;

        private void OnEnable()
        {
            if (health)
            {
                health.OnHealthChanged += UpdateHealth;
                UpdateHealth(health.Health, health.MaxHealth);
            }
            UpdateAmmoText();
        }

        private void OnDisable()
        {
            if (health) health.OnHealthChanged -= UpdateHealth;
        }

        private void UpdateHealth(float current, float max)
        {
            if (healthText) healthText.text = $"{Mathf.RoundToInt(current)} / {Mathf.RoundToInt(max)}";
            if (healthFill) healthFill.fillAmount = max > 0f ? current / max : 0f;
        }

        private void UpdateAmmoText()
        {
            if (ammoText) ammoText.text = $"{_inMag} / {_reserve}";
        }

        public void SetAmmo(int inMagazine, int reserve)
        {
            _inMag = Mathf.Max(0, inMagazine);
            _reserve = Mathf.Max(0, reserve);
            UpdateAmmoText();
        }

        public void OnShot()
        {
            if (_inMag > 0) _inMag--;
            UpdateAmmoText();
        }

        public void OnReload(int newInMag, int newReserve)
        {
            _inMag = Mathf.Max(0, newInMag);
            _reserve = Mathf.Max(0, newReserve);
            UpdateAmmoText();
        }
    }
}