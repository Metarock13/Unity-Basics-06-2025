using UnityEngine;

public class MedKitSpawner : MonoBehaviour
{
    public GameObject medkitPrefab;
    public Transform[] spawnPoints;

    private void Start()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            Instantiate(medkitPrefab, spawnPoint.position, medkitPrefab.transform.rotation);
        }
    }
}