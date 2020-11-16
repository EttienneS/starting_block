using System.Collections.Generic;
using UnityEngine;

namespace Assets.Map
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ChunkRenderer : MonoBehaviour
    {
        private List<Color> _colors;
        private Mesh _mesh;
        private MeshCollider _meshCollider;
        private List<int> _triangles;
        private List<Vector2> _uvs;
        private List<Vector3> _vertices;

        public ChunkCell[,] ChunkCells { get; set; }

        public int X { get; internal set; }

        public int Z { get; internal set; }

        public static ChunkRenderer CreateChunkRenderer(int x, int z, ChunkCell[,] cells)
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();

            var chunkRenderer = gameObject.AddComponent<ChunkRenderer>();
            chunkRenderer.X = x;
            chunkRenderer.Z = z;
            chunkRenderer.ChunkCells = cells;
            return chunkRenderer;
        }

        public void AddTriangle(int a, int b, int c)
        {
            _triangles.Add(a);
            _triangles.Add(b);
            _triangles.Add(c);
        }

     
        internal void SetMaterial(Material chunkMaterial)
        {
            GetComponent<MeshRenderer>().materials = new Material[] { chunkMaterial };
        }

        public void Start()
        {
            transform.position = GetPosition();

            GenerateMesh();
        }

        private void AddVert(float x, float y, float z, Color color)
        {
            _vertices.Add(new Vector3(x, y, z));
            _uvs.Add(new Vector2(x / GetWidth(), z / GetWidth()));
            _colors.Add(color);
        }

        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
            _mesh.name = $"Mesh {name}";
            _meshCollider = gameObject.AddComponent<MeshCollider>();
        }

        private void GenerateMesh()
        {
            _mesh.Clear();

            _uvs = new List<Vector2>();
            _vertices = new List<Vector3>();
            _colors = new List<Color>();
            _triangles = new List<int>();

            var i = 0;
            const int vertsPerCell = 4;

            var width = GetWidth();
            for (var z = 0; z < width; z++)
            {
                for (var x = 0; x < width; x++)
                {
                    var cell = ChunkCells[x, z];

                    var height = cell.Y;
                    AddVert(x - 0.5f, height, z - 0.5f, cell.Color);
                    AddVert(x + 0.5f, height, z - 0.5f, cell.Color);
                    AddVert(x + 0.5f, height, z + 0.5f, cell.Color);
                    AddVert(x - 0.5f, height, z + 0.5f, cell.Color);

                    var c = i * vertsPerCell;
                    AddTriangle(c + 3, c + 2, c + 1);
                    AddTriangle(c + 1, c, c + 3);

                    if (x < width - 1)
                    {
                        AddTriangle(c + 1, c + 2, c + 4);
                        AddTriangle(c + 4, c + 2, c + 7);
                    }

                    var cz = width * 4;
                    if (z < width - 1)
                    {
                        AddTriangle(c + 2, c + 3, c + cz + 1);
                        AddTriangle(c + cz + 1, c + 3, c + cz);
                    }

                    i++;
                }
            }

            _mesh.vertices = _vertices.ToArray();
            _mesh.colors = _colors.ToArray();
            _mesh.triangles = _triangles.ToArray();
            _mesh.uv = _uvs.ToArray();
            _mesh.RecalculateNormals();
            _meshCollider.sharedMesh = _mesh;
        }

        internal void Refresh()
        {
            GenerateMesh();
        }

        private Vector3 GetPosition()
        {
            return new Vector3(X * Constants.ChunkSize + 0.5f, 0, Z * Constants.ChunkSize + 0.5f);
        }

        private int GetWidth()
        {
            return Constants.ChunkSize;
        }
    }
}