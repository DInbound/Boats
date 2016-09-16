using UnityEngine;
using System.Collections;

namespace Surface
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SurfaceCreator : MonoBehaviour
    {
        private Mesh _mesh;
        private int _textureSize = 1;

        private Vector3[] vertices;
        private Vector3[] normals;
        private Color[] colors;

        public int size = 10;
        private int _currentSize;

        public Vector3 offset;
        public Vector3 rotation;

        public float frequency = 1f;

        [Range(1, 8)]
        public int octaves = 1;

        [Range(1f, 4f)]
        public float lacunarity = 2f;

        [Range(0f, 1f)]
        public float persistence = 0.5f;

        [Range(1, 3)]
        public int dimensions = 3;

        public NoiseMethodType type;

        public Gradient coloring;

        private void OnEnable()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.name = "Surface Mesh";
                GetComponent<MeshFilter>().mesh = _mesh;
            }
            Refresh();
        }

        public void Refresh()
        {
            if (size != _currentSize)
            {
                CreateGrid();
            }

            Quaternion q = Quaternion.Euler(rotation);
            Vector3 point00 = q * new Vector3(-0.5f, -0.5f) + offset;
            Vector3 point10 = q * new Vector3(0.5f, -0.5f) + offset;
            Vector3 point01 = q * new Vector3(-0.5f, 0.5f) + offset;
            Vector3 point11 = q * new Vector3(0.5f, 0.5f) + offset;

            NoiseMethod method = Noise.noiseMethods[(int)type][dimensions - 1];
            float stepSize = 1f / size;
            for (int v = 0, y = 0; y <= size; y++)
            {
                Vector3 point0 = Vector3.Lerp(point00, point01, y * stepSize);
                Vector3 point1 = Vector3.Lerp(point10, point11, y * stepSize);
                for (int x = 0; x <= size; x++, v++)
                {
                    Vector3 point = Vector3.Lerp(point0, point1, x * stepSize);
                    float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);
                    sample = type == NoiseMethodType.Value ? (sample - 0.5f) : (sample * 0.5f);
                    if (type != NoiseMethodType.Value)
                    {
                        sample = sample * 0.5f + 0.5f;
                    }
                    vertices[v].y = sample;
                    colors[v] = coloring.Evaluate(sample + 0.5f);
                }
            }
            _mesh.vertices = vertices;
            _mesh.colors = colors;
        }

        private void CreateGrid()
        {
            _currentSize = size;
            _mesh.Clear();

            float _stepSize = _textureSize;

            // Go over the verticies
            vertices = new Vector3[(size + 1) * (size + 1)];
            colors = new Color[vertices.Length];
            normals = new Vector3[vertices.Length];
            Vector2[] uv = new Vector2[vertices.Length];

            for (int v = 0, z = 0; z <= size; z++)
            {
                for (int x = 0; x <= size; x++, v++)
                {
                    vertices[v] = new Vector3(x * _stepSize - 0.5f, 0f, z * _stepSize - 0.5f);
                    colors[v] = Color.black;
                    normals[v] = Vector3.up;
                    uv[v] = new Vector3(x * _stepSize, z * _stepSize);
                }
            }
            _mesh.vertices = vertices;
            _mesh.colors = colors;
            _mesh.normals = normals;
            _mesh.uv = uv;

            // Go over the triangles
            int[] triangles = new int[size * size * 6];
            for (int t = 0, v = 0, y = 0; y < size; y++, v++)
            {
                for (int x = 0; x < size; x++, v++, t += 6)
                {
                    triangles[t] = v;
                    triangles[t + 1] = v + size + 1;
                    triangles[t + 2] = v + 1;
                    triangles[t + 3] = v + 1;
                    triangles[t + 4] = v + size + 1;
                    triangles[t + 5] = v + size + 2;
                }
            }
            _mesh.triangles = triangles;
        }
    }
}

