using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyController: MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 5f; // Mouse rotation sensitivity
    public float smoothVelocityTime = 0.1f; // Smoothing time for velocity transitions

    private float verticalRotation = 0f; // To store the current vertical rotation (pitch)

    private Rigidbody rb;
    private Vector3 currentVelocity;
    private Vector3 velocitySmoothDamp;

    private Camera playerCamera;
    public Material targetMaterial; // The material with the shader

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        SendVelocityToShader();
    }

    void HandleMovement()
    {
        // Input for movement
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrow keys
        float moveZ = Input.GetAxisRaw("Vertical"); // W/S or Up/Down arrow keys

        // Target velocity
        Vector3 targetVelocity = new Vector3(moveX, 0, moveZ).normalized * moveSpeed;

        // Smooth the velocity over time
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref velocitySmoothDamp, smoothVelocityTime);

        // Apply movement to rigidbody
        Vector3 move = transform.TransformDirection(currentVelocity);
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z); // Preserve vertical velocity (like for gravity)
    }

    void HandleMouseLook()
    {
        // Smooth mouse movement for looking around
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Apply horizontal rotation
        transform.Rotate(0, mouseX, 0);

        // Update vertical rotation (pitch) and clamp it between -90 and 90 degrees
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the camera's local rotation
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void SendVelocityToShader()
    {
        // If the material and shader are assigned, update shader with velocity
        if (targetMaterial != null)
        {
            Vector3 velocity = rb.velocity;
            // Convert velocity into a vector suitable for shader (you can normalize or scale it as per shader requirements)
            targetMaterial.SetVector("velocity", new Vector4(velocity.x, velocity.y, velocity.z, 1.0f));
        }
    }
}
