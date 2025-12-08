using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float cameraPitch = 0f;
    private bool canJump = true;

    private SnipeActions input;

    void Awake()
    {
        input = new SnipeActions();

        input.Move.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Move.Movement.canceled += ctx => moveInput = Vector2.zero;

        input.Move.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Move.LookAround.canceled += ctx => lookInput = Vector2.zero;

        input.Move.Jump.performed += ctx => Jump();
    }

    void OnEnable() => input.Move.Enable();
    void OnDisable() => input.Move.Disable();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent physics from rotating the player
    }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Look();
    }

    void Move()
    {
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        Vector3 velocity = move * speed;
        velocity.y = rb.linearVelocity.y; // keep vertical velocity
        rb.linearVelocity = velocity;
    }

    void Look()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        // rotate player horizontally
        transform.Rotate(Vector3.up, mouseX);

        // rotate camera vertically
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
    }

    void Jump()
    {
        if (canJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            canJump = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // allow jumping again when touching the ground
        if (collision.contacts[0].normal.y > 0.5f)
            canJump = true;
    }
}
