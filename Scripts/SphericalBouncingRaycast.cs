using UnityEngine;

public class SphericalBouncingRaycast : MonoBehaviour
{
    public Camera targetCamera;  // Camera to detect hits
    public int rayCount = 100;   // Number of rays to cast spherically
    public float rayDistance = 100f;  // Maximum distance for each ray
    public int maxBounces = 1;  // Number of allowed bounces

    void Update()
    {
        CastSphericalBouncingRays();
    }

    void CastSphericalBouncingRays()
    {
        Vector3 objectPosition = transform.position;  // Position of the object casting rays

        // Cast rays in a spherical pattern
        for (int i = 0; i < rayCount; i++)
        {
            // Get the direction from the Fibonacci sphere pattern
            Vector3 rayDirection = GetFibonacciSphereDirection(i, rayCount);

            // Cast the first ray from the object in the calculated direction
            CastRayWithBounces(objectPosition, rayDirection, 0);
        }
    }

    void CastRayWithBounces(Vector3 origin, Vector3 direction, int bounces)
    {
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        // Cast the ray and check if it hits anything
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            Debug.DrawRay(origin, direction * hit.distance, Color.blue);  // Draw the original ray

            // If we've hit something and we have bounces left
            if (bounces < maxBounces)
            {
                // Calculate the reflection direction
                Vector3 reflectionDirection = Vector3.Reflect(direction, hit.normal);

                // Cast a new ray from the hit point using the reflection direction
                CastRayWithBounces(hit.point, reflectionDirection, bounces + 1);
            }
            else
            {
                // Check if the final bounce hits the camera
                if (hit.collider.gameObject == targetCamera.gameObject)
                {
                    Debug.Log("Ray bounced and hit the camera!");
                    Debug.DrawRay(hit.point, direction * rayDistance, Color.red);  // Draw the ray in red if it hits the camera
                }
                else
                {
                    Debug.DrawRay(hit.point, direction * rayDistance, Color.green);  // Draw the ray in green if it hits something else
                }
            }
        }
    }

    // Function to generate evenly distributed points on a sphere using the Fibonacci sphere algorithm
    Vector3 GetFibonacciSphereDirection(int index, int totalPoints)
    {
        float phi = Mathf.Acos(1 - 2 * (index + 0.5f) / totalPoints);
        float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * (index + 0.5f);

        float x = Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = Mathf.Cos(phi);

        return new Vector3(x, y, z).normalized;  // Normalize the vector to get a direction
    }
}
