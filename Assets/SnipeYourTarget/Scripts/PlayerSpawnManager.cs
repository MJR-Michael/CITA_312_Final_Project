using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform[] playerSpawnPoints;    // Assign Spawn Point objects in Inspector
    public GameObject playerPrefab;          // Player prefab

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (playerSpawnPoints.Length == 0)
        {
            Debug.LogError("No player spawn points assigned!");
            return;
        }

        int index = Random.Range(0, playerSpawnPoints.Length);
        Transform spawn = playerSpawnPoints[index];

        Instantiate(playerPrefab, spawn.position, spawn.rotation);
    }
}
