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
    }

    public float CalculateY(float x, float z)
    {
        // Y = a * Sin(b * x + c) + d
        // a = Amplitude
        // b = Period (Frequency)
        // c = Phase Shift (X Offset)
        // d = Y Offset
        return Amplitude * Mathf.Sin(Frequency * (x + Time.time * Speed)) + Amplitude * Mathf.Sin(Frequency * (z + Time.time * Speed));
    }
}
