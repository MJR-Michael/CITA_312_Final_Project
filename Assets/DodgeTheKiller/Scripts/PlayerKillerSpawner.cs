using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Points (Order Matters!)")]
    public Transform[] playerSpawnPoints; // 6 elements
    public Transform[] killerSpawnPoints; // 6 elements

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject killerPrefab;

    void Start()
    {
        SpawnPlayerAndKiller();
    }

    void SpawnPlayerAndKiller()
    {
        if (playerSpawnPoints.Length != killerSpawnPoints.Length)
        {
            Debug.LogError("Player and Killer spawn arrays must be the same size!");
            return;
        }

        int index = Random.Range(0, playerSpawnPoints.Length);

        // Spawn player
        Instantiate(playerPrefab,
                    playerSpawnPoints[index].position,
                    playerSpawnPoints[index].rotation);

        // Spawn killer
        Instantiate(killerPrefab,
                    killerSpawnPoints[index].position,
                    killerSpawnPoints[index].rotation);

        Debug.Log($"Spawning pair at index {index}");
    }
}
