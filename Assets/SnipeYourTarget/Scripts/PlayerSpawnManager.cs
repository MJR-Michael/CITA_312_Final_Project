using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform[] playerSpawnPoints;    // Assign Spawn Point objects in Inspector
    public BoxCollider[] correspondingWanderingAreas; // Same order as spawn points
    public GameObject playerPrefab;          // Player prefab
    public NPCSpawnManager npcManager;       // Reference to NPCSpawnManager

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (playerSpawnPoints.Length == 0 || correspondingWanderingAreas.Length == 0)
        {
            Debug.LogError("No player spawn points or wandering areas assigned!");
            return;
        }

        int index = Random.Range(0, playerSpawnPoints.Length);
        Transform spawn = playerSpawnPoints[index];

        // Spawn player
        Instantiate(playerPrefab, spawn.position, spawn.rotation);

        // Spawn NPCs around the center of the corresponding wandering area
        BoxCollider area = correspondingWanderingAreas[index];
        Vector3 npcSpawnPosition = area.transform.position + area.center;

        npcManager.SpawnNPCs(npcSpawnPosition);
    }
}
