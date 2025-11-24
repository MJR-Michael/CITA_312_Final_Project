using UnityEngine;

public class BallKick : MonoBehaviour
{
    public Rigidbody rb; // Reference to the ball's Rigidbody
    public Collider goalArea; // The goal area (an invisible collider around the net)
    public GoalieGameManager gameManager; // Reference to the game manager to handle the lose sequence
    public float kickForce = 500f; // How hard the ball gets kicked

    // When the ball is kicked, it should move to a random point in the goal
    void Start()
    {
        KickBall();
    }

    // Kick the ball towards a random point inside the goal
    void KickBall()
    {
        // Get a random point inside the goal area (Collider.bounds gives the bounds of the collider)
        Vector3 randomPoint = new Vector3(
            Random.Range(goalArea.bounds.min.x, goalArea.bounds.max.x),
            Random.Range(goalArea.bounds.min.y, goalArea.bounds.max.y),
            Random.Range(goalArea.bounds.min.z, goalArea.bounds.max.z)
        );

        // Calculate direction towards that point
        Vector3 direction = (randomPoint - transform.position).normalized;

        // Apply force to the ball towards that point
        rb.AddForce(direction * kickForce);
    }

    // Detect when the ball exits the goal area (triggered when the ball leaves the collider)
    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the goal area is the soccer ball
        if (other.CompareTag("SoccerBall"))
        {
            // Trigger the lose sequence from the GameManager
            gameManager.TriggerLoseSequence();
        }
    }
}
