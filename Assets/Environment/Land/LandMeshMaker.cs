using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandMeshMaker : MonoBehaviour
{
    public Mesh mesh;

    [Range(1, 100)]
    public int sizeX = 1;
    [Range(1, 100)]
    public int sizeY = 1;
    [Range(1, 20)]
    public int Frequency = 10;
    [Range(1, 10)]
    public int Amplitude = 5;

    void Start()
    {
        GenerateMesh();
    }

    void Update()
    {
        //GenerateMesh();
    }

    private void GenerateMesh()
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> indices = new List<int>();

        int count = 0;

        // O(n2)
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                // tri 1
                verts.Add(new Vector3(i, CalculateY(i, j), j));
                verts.Add(new Vector3(i, CalculateY(i, j + 1), j + 1));
                verts.Add(new Vector3(i + 1, CalculateY(i + 1, j), j));
                // tri 2
                verts.Add(new Vector3(i, CalculateY(i, j + 1), j + 1));
                verts.Add(new Vector3(i + 1, CalculateY(i + 1, j + 1), j + 1));
                verts.Add(new Vector3(i + 1, CalculateY(i + 1, j), j));

                for (int k = 0; k < 6; k++)
                {
                    indices.Add(count);
                    normals.Add(Vector3.up);
                    count++;
                }
            }
        }

        mesh = new Mesh();

        mesh.vertices = verts.ToArray();
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
        mesh.normals = normals.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public float CalculateY(Vector3 pos)
    {
        return calcY(pos.x, pos.z);
    }

    public float CalculateY(float x, float z)
    {
        return calcY(x, z);
    }

    private float calcY(float x, float z)
    {
        return Mathf.PerlinNoise(x / Frequency, z / Frequency) * Amplitude;
    }
}
