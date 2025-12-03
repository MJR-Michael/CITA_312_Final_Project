using UnityEngine;
using UnityEngine.InputSystem;

public class GoalieMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Movement speed
    private float horizontalInput;

    void Update()
    {
        horizontalInput = Keyboard.current.aKey.isPressed ? -1f : (Keyboard.current.dKey.isPressed ? 1f : 0f);
        
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
    }
}
