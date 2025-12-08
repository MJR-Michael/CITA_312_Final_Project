using UnityEngine;

public class BallShooter : MonoBehaviour
{
    [Header("Assign the Field Net Collider")]
    public BoxCollider netCollider;

    [Header("Speed Settings")]
    public float minSpeed = 5f;
    public float maxSpeed = 15f;

    [Header("Random Delay Before Shooting")]
    public float minDelay = 3f;
    public float maxDelay = 7f;

    [Header("Game Manager")]
    public GoalieGameManager gameManager;

    private Rigidbody rb;
    private bool hasTriggeredResult = false; // Prevent double results
    private float shootTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        float delay = Random.Range(minDelay, maxDelay);
        Invoke(nameof(ShootBall), delay);
    }

    void ShootBall()
    {
        Vector3 targetPoint = GetRandomPointInBox(netCollider);
        Vector3 direction = (targetPoint - transform.position).normalized;
        float speed = Random.Range(minSpeed, maxSpeed);

        rb.linearVelocity = direction * speed;  // Updated for Unity 6+

        shootTime = Time.time;

        Invoke(nameof(CheckForNoCollision), 3f);
    }

    void CheckForNoCollision()
    {
        if (hasTriggeredResult) return;

        // 3 seconds passed with no collision
        hasTriggeredResult = true;
        gameManager.Win();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasTriggeredResult) return;

        // If ball hits the net → Game Over
        if (collision.collider == netCollider)
        {
            hasTriggeredResult = true;
            gameManager.GameOver();
            return;
        }

        // If ball hits the goalie → Win and attach ball
        if (collision.collider.CompareTag("Goalie"))
        {
            hasTriggeredResult = true;
            gameManager.Win();

            // Stop all physics so the ball doesn't fall or jitter
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            // Make the ball a child of the goalie
            transform.SetParent(collision.collider.transform);

            // Optional: Move ball slightly into the goalie’s hand
            // transform.localPosition = new Vector3(0, 1, 0.5f);

            return;
        }
    }


    Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 localCenter = box.center;
        Vector3 size = box.size;

        Vector3 randomLocalPoint = new Vector3(
            Random.Range(-size.x / 2f, size.x / 2f),
            Random.Range(-size.y / 2f, size.y / 2f),
            Random.Range(-size.z / 2f, size.z / 2f)
        );

        return box.transform.TransformPoint(localCenter + randomLocalPoint);
    }
}
