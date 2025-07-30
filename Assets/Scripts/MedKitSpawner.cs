using UnityEngine;

namespace Scripts
{
    public class MedKitSpawner : MonoBehaviour
    {
        public MedKit medkitPrefab;
        public Transform[] spawnPoints;

        private void Start()
        {
            foreach (var spawnPoint in spawnPoints)
            {
                Instantiate(medkitPrefab, spawnPoint.position, medkitPrefab.transform.rotation);
            }
        }
    }
}