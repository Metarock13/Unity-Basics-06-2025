using Code.Gameplay.Features.Enemy;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (enemyHealth != null)
            enemyHealth.OnHealthChanged += UpdateBar;

        UpdateBar(enemyHealth ? enemyHealth.CurrentHP: 0,
            enemyHealth ? enemyHealth.MaxHP : 1);
    }

    private void LateUpdate()
    {
        if (mainCamera)
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);

        if (enemyHealth)
            transform.position = enemyHealth.transform.position + offset;
    }

    private void UpdateBar(float current, float max)
    {
        if (fillImage)
            fillImage.fillAmount = max > 0 ? current / max : 0;
    }
}