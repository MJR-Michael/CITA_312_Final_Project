using UnityEngine;
using UnityEngine.AI;

public class KillerChase : MonoBehaviour
{
    public Transform player;     // Drag your player into this field
    public float catchDistance = 1.5f;  // Distance needed to trigger game over

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null)
        {
            // Chase the player
            agent.SetDestination(player.position);

            // Check if caught player
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= catchDistance)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        // Stop enemy movement
        agent.isStopped = true;

        // You can replace this with any game over logic (UI, reload scene, etc.)
        Debug.Log("GAME OVER!");
        
        // Example: reload the scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
