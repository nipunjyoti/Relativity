using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float maxSpeed = 5.0f;
    public Material material; // Assign the material with the shader in the inspector
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveInputV = Input.GetAxis("Vertical");
        float moveInputH = Input.GetAxis("Horizontal");
        Debug.Log(Input.GetAxis("Horizontal"));

        if (moveInputV == 0 || moveInputH == 0)
        {
            // Reset velocity to zero when no input is given
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        if (moveInputV != 0)
        {
            // Move the object forward based on its rotation
            Vector3 movementV = maxSpeed * moveInputV * transform.forward;
            rb.velocity = new Vector3(movementV.x, rb.velocity.y, movementV.z);
        }
        if (moveInputH != 0)
        {
            // Move the object forward based on its rotation
            Vector3 movementH = maxSpeed * moveInputH * transform.right;
            rb.velocity = new Vector3(movementH.x, rb.velocity.y, movementH.z);
        }

        // Update the movement speed for the shader
        //material.SetFloat("speed", Mathf.Abs(moveInput) * maxSpeed);
        material.SetVector("velocity", rb.velocity);
    }
}