using UnityEngine;

namespace Scripts
{
    public class MedKit : MonoBehaviour
    {
        [SerializeField] private float healAmount;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<Player>(out var player))
                {
                    player.Heal(healAmount);
                    Destroy(gameObject);
                }
            }
        }
    }
}
