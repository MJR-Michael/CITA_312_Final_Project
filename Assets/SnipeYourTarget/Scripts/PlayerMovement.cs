using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    SnipeActions input;
    Vector2 moveInput;
    Vector2 lookInput;
    float cameraPitch = 0f;

    void Awake()
    {
        input = new SnipeActions();

        input.Move.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Move.Movement.canceled += ctx => moveInput = Vector2.zero;

        input.Move.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Move.LookAround.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnEnable() => input.Move.Enable();
    void OnDisable() => input.Move.Disable();

    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        Vector3 move = transform.forward * moveInput.y +
                       transform.right * moveInput.x;

        transform.position += move * speed * Time.deltaTime;
    }

    void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        // Horizontal rotation (player body)
        transform.Rotate(Vector3.up, mouseX);

        // Vertical rotation (camera only)
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);

        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
    }
}
