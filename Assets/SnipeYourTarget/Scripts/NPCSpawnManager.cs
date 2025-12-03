using UnityEngine;

public class NPCSpawnManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject npcBasePrefab;          // Base NPC prefab with NavMeshAgent & NPCMovementBehavior
    public GameObject[] npcModelPrefabs;      // Visual model prefabs

    [Header("Counts")]
    public int npcCount = 10;

    [Header("Result (Debug)")]
    public GameObject targetNPC;
    public GameObject targetModel;

    /// <summary>
    /// Spawn NPCs for a specific player area.
    /// NPCs will wander around their spawn point using wanderRadius in NPCMovementBehavior.
    /// </summary>
    public void SpawnNPCs(Vector3 playerSpawnPosition)
    {
        if (npcModelPrefabs.Length < 2)
        {
            Debug.LogError("Need at least 2 NPC models (1 unique + 1 shared)");
            return;
        }

        // Pick a unique model for the target NPC
        GameObject uniqueModel = npcModelPrefabs[Random.Range(0, npcModelPrefabs.Length)];
        targetModel = uniqueModel;

        // Spawn the target NPC at the player spawn position
        targetNPC = SpawnSingleNPC(playerSpawnPosition, uniqueModel);

        // Spawn the remaining NPCs nearby
        for (int i = 1; i < npcCount; i++)
        {
            // Pick a model, but avoid the unique target model
            GameObject randomModel;
            do
            {
                randomModel = npcModelPrefabs[Random.Range(0, npcModelPrefabs.Length)];
            }
            while (randomModel == uniqueModel);

            // Spawn within 1–2 units radius around player spawn to avoid stacking exactly
            Vector3 spawnPos = playerSpawnPosition + new Vector3(
                Random.Range(-2f, 2f),
                0f,
                Random.Range(-2f, 2f)
            );

            SpawnSingleNPC(spawnPos, randomModel);
        }
    }

    GameObject SpawnSingleNPC(Vector3 spawnPos, GameObject modelPrefab)
    {
        GameObject npc = Instantiate(npcBasePrefab, spawnPos, Quaternion.identity);

        // Spawn model under NPC
        GameObject modelInstance = Instantiate(modelPrefab, npc.transform);
        modelInstance.transform.localPosition = Vector3.zero;

        // The new NPCMovementBehavior already uses the spawn position,
        // so no need to assign a BoxCollider or call SetWanderArea

        return npc;
    }
}
