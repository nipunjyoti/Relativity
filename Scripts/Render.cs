using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.RecalculateBounds();
        mesh.bounds = new Bounds(Vector3.zero, 1000f * Vector3.one);
    }

}
