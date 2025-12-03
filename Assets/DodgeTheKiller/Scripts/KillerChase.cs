using UnityEngine;
using UnityEngine.AI;

public class KillerChase : MonoBehaviour
{
    public Transform player;     // Drag your player into this field
    public float catchDistance = 1.5f;  // Distance needed to trigger game over
    public float turnSpeed = 10f;       // Speed at which the enemy turns (adjust for tighter turns)
    public float angularSpeed = 500f;
    public float acceleration = 10f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Adjust NavMeshAgent properties for tight turns
        agent.angularSpeed = angularSpeed;    // Faster turning
        agent.acceleration = acceleration;     // More acceleration
        agent.stoppingDistance = catchDistance; // Adjust to stop when near the player
        agent.radius = 0.5f;          // Smaller radius for tighter navigation
    }

    void Update()
    {
        if (player != null)
        {
            // Get the position of the player, ignoring Y-axis
            Vector3 targetPosition = player.position;
            targetPosition.y = transform.position.y; // Keep enemy on same Y level

            // Set the destination of the NavMeshAgent towards the player (X and Z only)
            agent.SetDestination(targetPosition);

            // Calculate the distance to the player
            float distance = Vector3.Distance(transform.position, player.position);

            // If the enemy is close enough to the player, trigger Game Over
            if (distance <= catchDistance)
            {
                GameOver();
            }

            // Smoothly rotate the enemy towards the player
            RotateTowardsPlayer(targetPosition);
        }
    }

    void RotateTowardsPlayer(Vector3 targetPosition)
    {
        // Calculate direction to player on X and Z axis
        Vector3 directionToPlayer = targetPosition - transform.position;
        directionToPlayer.y = 0; // Ignore Y-axis rotation

        if (directionToPlayer.sqrMagnitude > 0.01f) // Check if the player is far enough to rotate
        {
            // Smoothly rotate towards the player on the X and Z axes
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    void GameOver()
    {
        // Stop enemy movement
        agent.isStopped = true;

        // Trigger game over logic (e.g., show UI, reload scene, etc.)
        Debug.Log("GAME OVER!");

        // Example: Reload the scene (optional, comment out if not needed)
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
