using UnityEngine;

public class RenderObjectOutsideFOV : MonoBehaviour
{
    public Camera cam;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    void Start()
    {
        // Get the MeshRenderer and MeshFilter components of the object
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        // Get the frustum planes of the camera
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

        // Check if the object's bounds are within the camera's frustum
        bool isInView = GeometryUtility.TestPlanesAABB(planes, meshRenderer.bounds);

        if (!isInView)
        {
            // If the object is outside the frustum, manually render it
            RenderOutsideFOV();
        }
    }

    void RenderOutsideFOV()
    {
        // Manually draw the object using its mesh and material
        if (meshFilter && meshRenderer)
        {
            Graphics.DrawMesh(meshFilter.sharedMesh, transform.localToWorldMatrix, meshRenderer.sharedMaterial, 0);
        }
    }
}
