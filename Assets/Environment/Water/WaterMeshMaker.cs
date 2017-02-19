using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterMeshMaker : MonoBehaviour
{
    private Mesh _mesh;

    private List<Vector3> verts = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<int> indices = new List<int>();

    [Range(1, 100)]
    public int sizeX = 100;
    [Range(1, 100)]
    public int sizeY = 100;
    [Range(0, 1)]
    public float Amplitude;
    [Range(0, 1)]
    public float Frequency;
    [Range(0, 5)]
    public float Speed;

    void Start()
    {
        int count = 0;

        // O(n2)
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                // tri 1
                verts.Add(new Vector3(i, 0, j));
                verts.Add(new Vector3(i, 0, j + 1));
                verts.Add(new Vector3(i + 1, 0, j));
                // tri 2
                verts.Add(new Vector3(i, 0, j + 1));
                verts.Add(new Vector3(i + 1, 0, j + 1));
                verts.Add(new Vector3(i + 1, 0, j));

                for (int k = 0; k < 6; k++)
                {
                    indices.Add(count);
                    normals.Add(Vector3.up);
                    count++;
                }
            }
        }

        _mesh = new Mesh();

        _mesh.vertices = verts.ToArray();
        _mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
        _mesh.normals = normals.ToArray();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    void Update()
    {
        float x = Raycast(new Vector3(0f, 10f, 0f));
    }

    /// <summary>
    /// Return the Y value of the water at a specific x and z position.
    /// </summary>
    /// <param name="x">X position</param>
    /// <param name="z">Z position</param>
    /// <returns>A certain height of water.</returns>
    public float CalculateY(float x, float z)
    {
        return Sine(x, z);
    }

    private float Sine(float x, float z)
    {
        float returnThis = 0;

        float random = Rand(new Vector3(x, 0, z));
        returnThis += Mathf.Sin(Time.time * Speed + x * Frequency * Frequency) * Amplitude;
        return returnThis;
    }

    /// <summary>
    /// Should be the same as in the shader.
    /// </summary>
    /// <returns>a semi random value</returns>
    private float Rand(Vector3 vec)
    {
        // TODO FIXME HALP: do fractal thingy.
        // return frac(sin(dot(myVector, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
        float x = Mathf.Sin(  Vector3.Dot(vec, new Vector3(12.9898f, 78.233f, 45.5432f))) * 43758.5453f;

        return x - Mathf.Floor(x);
    }

    public float Raycast(Vector3 position)
    {
        RaycastHit hitInfo = new RaycastHit();
        float aDistance = 9999;
        int layerMask = 1 << 4;

        if(Physics.Raycast(position, transform.TransformDirection(Vector3.down), 100f, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            aDistance = hitInfo.distance;
        }
        return aDistance;
    }
}
