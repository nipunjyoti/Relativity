using UnityEngine;
using System.Collections.Generic;

public class Raycast : MonoBehaviour
{
    public Camera targetCamera;      // Camera to detect hits
    public int rayCount = 100;       // Number of rays to cast spherically
    public float rayDistance = 100f; // Maximum distance for each ray
    public int maxBounces = 1;       // Number of allowed bounces
    public float raySpeed = 20f;     // Speed of the rays
    public GameObject objectToRender; // Object that will be rendered when rays hit the camera

    private List<RayData> rays = new List<RayData>(); // List to keep track of all active rays

    void Start()
    {
        CastInitialSphericalRays();  // Initialize rays when the scene starts
        objectToRender.GetComponent<Renderer>().enabled = false; // Initially, disable the object's rendering
    }

    void Update()
    {
        CastInitialSphericalRays();
        MoveRays();  // Update the rays every frame, simulating their movement
    }

    void CastInitialSphericalRays()
    {
        Vector3 objectPosition = transform.position;  // Position of the object casting rays

        // Cast rays in a spherical pattern
        for (int i = 0; i < rayCount; i++)
        {
            // Get the direction from the Fibonacci sphere pattern
            Vector3 rayDirection = GetFibonacciSphereDirection(i, rayCount);

            // Create a new ray and add it to the list
            RayData newRay = new RayData(objectPosition, rayDirection, 0);
            rays.Add(newRay);
        }
    }

    void MoveRays()
    {
        // Loop through all active rays and move them
        for (int i = 0; i < rays.Count; i++)
        {
            RayData ray = rays[i];

            // Move the ray based on speed and time
            ray.currentPosition += ray.direction * raySpeed * Time.deltaTime;

            RaycastHit hit;
            if (Physics.Raycast(ray.previousPosition, ray.direction, out hit, Vector3.Distance(ray.previousPosition, ray.currentPosition)))
            {
                Debug.DrawRay(ray.previousPosition, ray.direction * hit.distance, Color.blue);  // Draw the ray up to the hit point

                // If the ray hits an object, check for bounces or the camera
                if (ray.bounces < maxBounces)
                {
                    // Calculate the reflection direction and add a bounce
                    ray.direction = Vector3.Reflect(ray.direction, hit.normal);
                    ray.bounces++;
                    ray.currentPosition = hit.point;  // Set the position at the hit point for the next bounce
                }
                else if (hit.collider.gameObject == targetCamera.gameObject)
                {
                    Debug.Log("Ray hit the camera!");
                    Debug.DrawRay(hit.point, ray.direction * rayDistance, Color.red);  // Draw in red if it hits the camera

                    // Now render the object that the ray last hit
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject != null)
                    {
                        Renderer objectRenderer = hitObject.GetComponent<Renderer>();
                        if (objectRenderer != null)
                        {
                            objectRenderer.enabled = true; // Enable rendering for the object
                        }
                    }
                }
                else
                {
                    Debug.DrawRay(hit.point, ray.direction * rayDistance, Color.green);  // Draw in green if it hits something else
                }
            }

            // Update the previous position for the next frame
            ray.previousPosition = ray.currentPosition;
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

    // Data structure to keep track of each ray's state
    private class RayData
    {
        public Vector3 currentPosition;  // Current position of the ray
        public Vector3 previousPosition; // Previous position of the ray
        public Vector3 direction;        // Direction the ray is traveling
        public int bounces;              // Number of bounces this ray has done

        // Constructor
        public RayData(Vector3 startPosition, Vector3 direction, int bounces)
        {
            this.currentPosition = startPosition;
            this.previousPosition = startPosition;
            this.direction = direction;
            this.bounces = bounces;
        }
    }
}
