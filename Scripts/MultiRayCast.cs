using UnityEngine;

public class MultiRayCast : MonoBehaviour
{
    public Transform lightSphere;
    public Material planeMaterial;
    public int numRays = 10;
    public float lightSpeed = 10f;

    void Update()
    {
        Vector4[] lightPositions = new Vector4[numRays];
        Vector4[] rayDirections = new Vector4[numRays];

        for (int i = 0; i < numRays; i++)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            lightPositions[i] = lightSphere.position;
            rayDirections[i] = randomDirection;
        }

        planeMaterial.SetVectorArray("_LightPositions", lightPositions);
        planeMaterial.SetVectorArray("_RayDirections", rayDirections);
        planeMaterial.SetFloat("_LightSpeed", lightSpeed);
    }
}