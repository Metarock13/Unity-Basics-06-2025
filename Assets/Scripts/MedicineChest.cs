using UnityEngine;

namespace Scripts
{
    public class MedicineChest : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HealthController healthController))
            {
                if (healthController.CanAddHealth())
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
