using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;
    public int spawnCount = 6;
    public Vector3 spawnArea = new Vector3(20,0,20);

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-spawnArea.x/2, spawnArea.x/2), 0.5f, Random.Range(-spawnArea.z/2, spawnArea.z/2));
            Instantiate(pickupPrefab, pos, Quaternion.identity);
        }
    }
}
