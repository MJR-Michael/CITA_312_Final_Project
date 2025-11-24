using UnityEngine;

public class GoalieBlock : MonoBehaviour
{
    public Rigidbody rb; // Goalie's Rigidbody
    public float catchForce = 500f; // Force to stop the ball or catch it

    // Detect collision with ball
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the ball collided with the goalie
        if (collision.gameObject.CompareTag("SoccerBall"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            
            // Stop the ball's movement (catch it)
            if (ballRb != null)
            {
                // Option 1: Stop the ball by applying opposite force
                ballRb.linearVelocity = Vector3.zero;

                // Option 2: Apply a catching force to slow down and stop it
                ballRb.AddForce(-ballRb.linearVelocity * catchForce);

                // Optionally: you can "catch" the ball by parenting it to the goalie
                collision.transform.parent = this.transform;
            }
        }
    }
}
