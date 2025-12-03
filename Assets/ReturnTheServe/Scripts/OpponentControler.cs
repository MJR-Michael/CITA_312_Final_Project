using UnityEngine;

public class OpponentController : MonoBehaviour
{
    [Header("References")]
    public Transform player;     // drag the Player object here
    public Transform courtCenter; // empty GameObject in the center of the court

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float mirrorStrength = 1f;  // 1 = perfect mirror, <1 = looser tracking

    void Update()
    {
        if (player == null || courtCenter == null)
            return;

        FacePlayer();
        MirrorHorizontalMovement();
    }

    void FacePlayer()
    {
        Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookPos);
    }

    void MirrorHorizontalMovement()
    {
        // How far the player is left/right from the court center
        float horizontalOffset = player.position.x - courtCenter.position.x;

        // Mirror the offset (opponent moves opposite side)
        float targetX = courtCenter.position.x - (horizontalOffset * mirrorStrength);

        Vector3 targetPos = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }
}
