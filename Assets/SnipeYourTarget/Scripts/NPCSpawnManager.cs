using UnityEngine;
using System.Collections.Generic;

public class NPCSpawnManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject npcBasePrefab;                  // Contains AI, movement, empty slot for model
    public GameObject[] npcModelPrefabs;              // Visual model prefabs
    public Transform[] npcSpawnPoints;                // Where NPCs can spawn
    public Transform modelAttachPoint;                // Where model gets placed on NPC
    public BoxCollider[] wanderingAreas;                 // NPC movement limits

    [Header("Counts")]
    public int npcCount = 10;

    [Header("Result (Debug)")]
    public GameObject targetNPC;
    public GameObject targetModel;

    void Start()
    {
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
        if (npcSpawnPoints.Length == 0)
        {
            Debug.LogError("No NPC Spawn Points assigned!");
            return;
        }

        if (npcModelPrefabs.Length < 2)
        {
            Debug.LogError("Need at least 2 NPC models (1 unique + 1 shared)");
            return;
        }

        // Pick a unique model for the target NPC
        GameObject uniqueModel = npcModelPrefabs[Random.Range(0, npcModelPrefabs.Length)];
        targetModel = uniqueModel;

        // Spawn the target NPC first
        Transform targetSpawn = npcSpawnPoints[Random.Range(0, npcSpawnPoints.Length)];
        targetNPC = SpawnSingleNPC(targetSpawn, uniqueModel);

        // Now spawn the rest
        for (int i = 1; i < npcCount; i++)
        {
            Transform spawn = npcSpawnPoints[Random.Range(0, npcSpawnPoints.Length)];

            // Pick a model, but avoid the unique target model
            GameObject randomModel;
            do
            {
                randomModel = npcModelPrefabs[Random.Range(0, npcModelPrefabs.Length)];
            }
            while (randomModel == uniqueModel); // Prevent duplicates of the unique model

            SpawnSingleNPC(spawn, randomModel);
        }
    }

    GameObject SpawnSingleNPC(Transform spawnPoint, GameObject modelPrefab)
    {
        GameObject npc = Instantiate(npcBasePrefab, spawnPoint.position, spawnPoint.rotation);

        // Spawn model under NPC
        GameObject modelInstance = Instantiate(modelPrefab, npc.transform);
        modelInstance.transform.localPosition = Vector3.zero;

        // Assign wandering area
        BoxCollider area = wanderingAreas[Random.Range(0, wanderingAreas.Length)];
        npc.GetComponent<NPCMovementBehavior>().SetWanderArea(area);

        return npc;
    }
}
