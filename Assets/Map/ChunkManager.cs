using Assets.Map.Pathing;
using UnityEngine;

namespace Assets.Map
{
    public class ChunkManager : MonoBehaviour
    {
        public Material ChunkMaterial;

        private Cell[,] _cells;
        private ChunkRenderer[,] _chunkRenderers;

        public Pathfinder Pathfinder { get; internal set; }

        public static ChunkManager CreateChunkManager(Material chunkMaterial, Transform parent = null)
        {
            var manager = new GameObject(nameof(ChunkManager))
                                     .AddComponent<ChunkManager>();
            manager.ChunkMaterial = chunkMaterial;

            if (parent != null)
            {
                manager.transform.SetParent(parent);
            }

            return manager;
        }

        public void DestroyChunks()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void RenderCells(Cell[,] cellsToRender)
        {
            DestroyChunks();
            _cells = cellsToRender;

            var width = Mathf.FloorToInt(cellsToRender.GetLength(0) / Constants.ChunkSize);
            var height = Mathf.FloorToInt(cellsToRender.GetLength(1) / Constants.ChunkSize);

            _chunkRenderers = new ChunkRenderer[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    _chunkRenderers[x, z] = MakeChunkRenderer(x, z);
                }
            }
            CreatePathfinder();
        }

        public void Start()
        {
            CreatePathfinder();
        }

        internal Cell[,] GetCells(int offsetX, int offsetY)
        {
            var cells = new Cell[Constants.ChunkSize, Constants.ChunkSize];
            offsetX *= Constants.ChunkSize;
            offsetY *= Constants.ChunkSize;

            for (var x = 0; x < Constants.ChunkSize; x++)
            {
                for (var y = 0; y < Constants.ChunkSize; y++)
                {
                    cells[x, y] = _cells[offsetX + x, offsetY + y];
                }
            }

            return cells;
        }

        private void CreatePathfinder()
        {
            var pf = new GameObject("Pathfinder");
            pf.transform.SetParent(transform);
            Pathfinder = pf.AddComponent<Pathfinder>();
        }

        private ChunkRenderer MakeChunkRenderer(int x, int z)
        {
            var renderer = ChunkRenderer.CreateChunkRenderer(x, z, GetCells(x, z));
            renderer.transform.SetParent(transform);
            renderer.name = $"{x} - {z}";

            renderer.SetMaterial(ChunkMaterial);

            return renderer;
        }

        public ChunkRenderer GetRendererForCell(Cell cell)
        {
            var x = Mathf.FloorToInt(cell.X / Constants.ChunkSize);
            var z = Mathf.FloorToInt(cell.Z / Constants.ChunkSize);

            return _chunkRenderers[x, z];
        }
    }
}