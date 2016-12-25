using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OceanGenerator : MonoBehaviour
{
    public Mesh mesh;

    public int sizeX;
    public int sizeY;

    public float waveHeight;
    public float waveFreq;

    void Start()
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> indices = new List<int>();

        int count = 0;
        for(int i = 0; i < sizeX; i++)
        {
            for(int j = 0; j < sizeY; j++)
            {
                // tri 1
                verts.Add(new Vector3(i, waveHeight * Mathf.PerlinNoise(i + waveFreq, j + waveFreq), j));
                verts.Add(new Vector3(i, waveHeight * Mathf.PerlinNoise(i + waveFreq, j + waveFreq + 1), j + 1));
                verts.Add(new Vector3(i + 1, waveHeight * Mathf.PerlinNoise(i + waveFreq + 1, j + waveFreq), j));
                // tri 2
                verts.Add(new Vector3(i, waveHeight * Mathf.PerlinNoise(i + waveFreq, j + waveFreq + 1), j + 1));
                verts.Add(new Vector3(i + 1, waveHeight * Mathf.PerlinNoise(i + waveFreq + 1, j + waveFreq + 1), j + 1));
                verts.Add(new Vector3(i + 1, waveHeight * Mathf.PerlinNoise(i + waveFreq + 1, j + waveFreq), j));

                for(int k = 0; k < 6; k++)
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

        GetComponent<MeshFilter>().mesh = mesh;
    }

    public float IsInWater(Vector3 position)
    {
        float waterHeight = waveHeight * Mathf.Sin(position.x + Time.time + waveFreq) * Mathf.Sin(position.z + Time.time + waveFreq);

        return waterHeight - position.y;
    }

    void Update()
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> indices = new List<int>();

        int count = 0;
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                //waveHeight * Mathf.PerlinNoise(i + waveFreq * Time.time, j + waveFreq * Time.time)
                // tri 1
                verts.Add(new Vector3(i, waveHeight * Mathf.Sin(i + Time.time + waveFreq) * Mathf.Sin(j + Time.time + waveFreq), j));
                verts.Add(new Vector3(i, waveHeight * Mathf.Sin(i + Time.time + waveFreq) * Mathf.Sin(j + 1 + Time.time + waveFreq), j + 1));
                verts.Add(new Vector3(i + 1, waveHeight * Mathf.Sin(i + 1 + Time.time + waveFreq) * Mathf.Sin(j + Time.time + waveFreq), j));
                // tri 2
                verts.Add(new Vector3(i, waveHeight * Mathf.Sin(i + Time.time + waveFreq) * Mathf.Sin(j + 1 + Time.time + waveFreq), j + 1));
                verts.Add(new Vector3(i + 1, waveHeight * Mathf.Sin(i + 1 + Time.time + waveFreq) * Mathf.Sin(j + 1 + Time.time + waveFreq), j + 1));
                verts.Add(new Vector3(i + 1, waveHeight * Mathf.Sin(i + 1 + Time.time + waveFreq) * Mathf.Sin(j + Time.time + waveFreq), j));

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
    }
}
