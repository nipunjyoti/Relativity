using UnityEngine;

public class RaycastToCamera : MonoBehaviour
{
    public Camera targetCamera;
    public int rayCount = 10; // Number of rays to cast
    public float raySpread = 5f; // Spread of the rays

    void Update()
    {
        RaycastRays();
    }

    void RaycastRays()
    {
        Vector3 objectPosition = transform.position; // Position of the object casting rays
        Vector3 cameraPosition = targetCamera.transform.position;

        for (int i = 0; i < rayCount; i++)
        {
            // Randomly spread the rays (you can adjust the pattern to suit your needs)
            Vector3 randomOffset = new Vector3(Random.Range(-raySpread, raySpread), Random.Range(-raySpread, raySpread), 0);
            Vector3 rayDirection = (cameraPosition + randomOffset) - objectPosition;

            Ray ray = new Ray(objectPosition, rayDirection);
            RaycastHit hit;

            // Cast a ray
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the ray hit the camera
                if (hit.collider.gameObject == targetCamera.gameObject)
                {
                    Debug.Log("Ray hit the camera!");
                    Debug.DrawRay(objectPosition, rayDirection, Color.red); // Draw the ray in the scene view
                }
                else
                {
                    Debug.DrawRay(objectPosition, rayDirection, Color.green); // If it hits something else
                }
            }
        }
    }
}
