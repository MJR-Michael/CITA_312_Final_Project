using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReturn : MonoBehaviour
{
    [Header("References")]
    public TennisBall ball;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Opponent Court Area (Drag BoxCollider)")]
    public BoxCollider opponentCourt;

    private Vector2 moveInput;

    void Update()
    {
        HandleMovement();
    }

    // -------------------------
    // MOVEMENT 
    // -------------------------
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    // -------------------------
    // AUTO RETURN ON COLLISION
    // -------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ReturnBall();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            ReturnBall();
        }
    }

    void ReturnBall()
    {
        if (ball == null || opponentCourt == null)
        {
            Debug.LogError("Ball or opponent court not assigned!");
            return;
        }

        Debug.Log("Player returned the ball!");

        Vector3 target = GetRandomPointInCourt(opponentCourt);
        ball.ReturnToTarget(target);
    }

    // Random point in opponent’s court
    Vector3 GetRandomPointInCourt(BoxCollider box)
    {
        Vector3 center = box.transform.TransformPoint(box.center);
        Vector3 size = box.size;
        Vector3 worldSize = Vector3.Scale(size, box.transform.lossyScale);

        float x = Random.Range(center.x - worldSize.x / 2f, center.x + worldSize.x / 2f);
        float z = Random.Range(center.z - worldSize.z / 2f, center.z + worldSize.z / 2f);

        return new Vector3(x, box.transform.position.y, z);
    }
}
