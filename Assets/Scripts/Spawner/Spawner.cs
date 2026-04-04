using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objPrefabs;
    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 3f; 

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            int index = Random.Range(0, objPrefabs.Length);
            GameObject prefabToSpawn = objPrefabs[index];
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
        }
    }
}
