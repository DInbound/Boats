using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SurfaceCreator : MonoBehaviour
{
    private Mesh _mesh;
    private int _textureSize = 1;

    [Range(1, 200)]
    public int Resolution = 10;
    private int _currentResolution;

    private void OnEnable()
    {
        if(_mesh == null)
        {
            _mesh = new Mesh();
            _mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = _mesh;
        }
        Refresh();
    }

    public void Refresh()
    {
        if(Resolution != _currentResolution)
        {
            CreateGrid();
        }
    }

    private void CreateGrid()
    {
        _currentResolution = Resolution;
        _mesh.Clear();

        float _stepSize = 1f / Resolution;

        // Go over the verticies
        Vector3[] vertices = new Vector3[(Resolution + 1) * (Resolution + 1)];
        Color[] colors = new Color[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int v = 0, z = 0; z <= Resolution; z++)
        {
            for (int x = 0; x <= Resolution; x++, v++)
            {
                vertices[v] = new Vector3(x * _stepSize - 0.5f, 0, z * _stepSize - 0.5f);
                colors[v] = Color.black;
                normals[v] = Vector3.back;
                uv[v] = new Vector3(x * _stepSize, 0, z * _stepSize);
            }
        }
        _mesh.vertices = vertices;
        _mesh.colors = colors;
        _mesh.normals = normals;
        _mesh.uv = uv;

        // Go over the triangles
        int[] triangles = new int[Resolution * Resolution * 6];
        for (int t = 0, v = 0, y = 0; y < Resolution; y++, v++)
        {
            for (int x = 0; x < Resolution; x++, v++, t += 6)
            {
                triangles[t] = v;
                triangles[t + 1] = v + Resolution + 1;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + Resolution + 1;
                triangles[t + 5] = v + Resolution + 2;
            }
        }
        _mesh.triangles = triangles;
    }
}
