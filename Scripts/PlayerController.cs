using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Speed of the player
    public float mouseSensitivity = 2f; // Sensitivity of the mouse
    public Transform cameraTransform; // Reference to the camera transform
    public Material material; // Assign the material with the shader in the inspector

    private Rigidbody rb;
    private float rotationX = 0f; // Rotation around the x-axis
    private float rotationY = 0f; // Rotation around the y-axis

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    void Update()
    {
        // Get mouse input for looking around
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Apply mouse movement to rotation
        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Clamp up/down rotation

        // Rotate the camera and player
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        // Get movement input
        float moveX = Input.GetAxis("Horizontal"); // A/D keys
        float moveZ = Input.GetAxis("Vertical"); // W/S keys

        if (moveZ != 0)
        {
            // Move the object forward based on its rotation
            Vector3 movementV = speed * moveZ * transform.forward;
            rb.velocity = new Vector3(movementV.x, rb.velocity.y, movementV.z);
        }
        if (moveX != 0)
        {
            // Move the object forward based on its rotation
            Vector3 movementH = speed * moveX * transform.right;
            rb.velocity = new Vector3(movementH.x, rb.velocity.y, movementH.z);
        }

        material.SetVector("velocity", rb.velocity);
    }
}
