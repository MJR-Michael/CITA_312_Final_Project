using UnityEngine;
using UnityEngine.AI;

public class NPCMovementBehavior : MonoBehaviour
{
    [Header("Wandering Settings")]
    public float wanderRadius = 60f;        // Max distance NPC can move from spawn
    public float wanderDelay = 5f;          // Time between picking new destinations
    public float stoppingDistance = 0.5f;   // How close to the destination before picking a new one
    public int maxAttempts = 20;            // Max tries to find a valid point on NavMesh

    private NavMeshAgent agent;
    private Vector3 spawnPosition;
    private float timer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!agent) Debug.LogError("NPCMovementBehavior requires a NavMeshAgent!");
        spawnPosition = transform.position; // remember spawn point
    }

    void Start()
    {
        timer = Random.Range(0f, wanderDelay); // stagger start
        PickNewDestination();
    }

    void Update()
    {
        if (!agent) return;

        timer += Time.deltaTime;

        if (timer >= wanderDelay || (!agent.pathPending && agent.remainingDistance <= stoppingDistance))
        {
            PickNewDestination();
            timer = 0f;
        }
    }

    void PickNewDestination()
    {
        NavMeshHit hit;
        bool found = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            // Pick a random point within a circle (XZ plane) around spawn
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            Vector3 randomPoint = spawnPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);

            if (NavMesh.SamplePosition(randomPoint, out hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                found = true;
                break;
            }
        }

        if (!found)
            Debug.LogWarning("Could not find a valid NavMesh point for NPC: " + name);
    }
}
