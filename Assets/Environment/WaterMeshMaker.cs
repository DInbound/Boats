using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterMeshMaker : MonoBehaviour
{
    public Mesh mesh;

    [Range(1, 100)]
    public int sizeX;
    [Range(1, 100)]
    public int sizeY;

    [Range(0.01f, 5)]
    public float WaveAmplitude = 1;
    [Range(0.01f, 5)]
    public float WaveFrequency = 1;
    [Range(0.01f, 5)]
    public float WaveSpeed = 1;

    void Start()
    {
    }

    void Update()
    {
        GenerateMesh();
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
    }

    /// <summary>
    /// Calculates the Y coördinate of the water according to a sine function with time and perlin noise.
    /// </summary>
    /// <param name="pos">The position of the point to calculate.</param>
    /// <returns>The Y value of the water at a specific point.</returns>
    public float CalculateY(Vector3 pos)
    {
        //waveHeight * Mathf.PerlinNoise(i + waveFreq * Time.time, j + waveFreq * Time.time)
        return calcY(pos.x, pos.z);
    }

    /// <summary>
    /// Calculates the Y coördinate of the water according to a sine function with time and perlin noise.
    /// </summary>
    /// <param name="x">The x-coordinate of the point.</param>
    /// <param name="z">The z-coordinate of the point.</param>
    /// <returns>The Y value of the water at a specific point.</returns>
    public float CalculateY(float x, float z)
    {
        return calcY(x, z);
    }

    private float calcY(float x, float z)
    {
        // Y = a * Sin(b * x + c) + d
        // a = Amplitude
        // b = Period (Frequency)
        // c = Phase Shift (X Offset)
        // d = Y Offset
        return WaveAmplitude * Mathf.Sin(WaveFrequency * (x + Time.time * WaveSpeed)) + WaveAmplitude * Mathf.Sin(WaveFrequency * (z + Time.time * WaveSpeed));
    }
}
