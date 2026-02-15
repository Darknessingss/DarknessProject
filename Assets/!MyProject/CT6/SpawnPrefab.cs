using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (prefabToSpawn != null)
            {
                if (spawnPoint != null)
                {
                    Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
                }
                else
                {
                    Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
                }

                Debug.Log($"Префаб {prefabToSpawn.name} создан");
            }
        }
    }
}