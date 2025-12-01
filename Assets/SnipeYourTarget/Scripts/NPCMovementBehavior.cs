using UnityEngine;
using UnityEngine.AI;

public class NPCMovementBehavior : MonoBehaviour
{
    public BoxCollider wanderArea;
    public float wanderDelay = 3f;

    NavMeshAgent agent;
    float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PickNewDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderDelay || agent.remainingDistance < 0.5f)
        {
            PickNewDestination();
            timer = 0f;
        }
    }

    public void SetWanderArea(BoxCollider area)
    {
        wanderArea = area;
    }

    void PickNewDestination()
    {
        if (!wanderArea)
            return;

        Vector3 point = GetRandomPointInBox(wanderArea);

        agent.SetDestination(point);
    }

    Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 center = box.transform.position + box.center;
        Vector3 size = box.size;

        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(x, y, z);
    }
}
