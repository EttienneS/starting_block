using Assets.Map.Pathing;
using UnityEngine;

namespace Assets.Map
{
    public class ChunkManager : MonoBehaviour
    {
        public Material ChunkMaterial;

        private ChunkCell[,] _cells;
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

        public void RenderCells(ChunkCell[,] cellsToRender)
        {
            _cells = cellsToRender;

            var width = Mathf.FloorToInt(cellsToRender.GetLength(0) / Constants.ChunkSize);
            var height = Mathf.FloorToInt(cellsToRender.GetLength(1) / Constants.ChunkSize);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    MakeChunkRenderer(x, z);
                }
            }
        }

        public void Start()
        {
            CreatePathfinder();
        }

        internal ChunkCell[,] GetCells(int offsetX, int offsetY)
        {
            var cells = new ChunkCell[Constants.ChunkSize, Constants.ChunkSize];
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

        public void DestroyChunks()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

      

        private void MakeChunkRenderer(int x, int z)
        {
            var renderer = ChunkRenderer.CreateChunkRenderer(x, z, GetCells(x, z));
            renderer.transform.SetParent(transform);
            renderer.name = $"{x} - {z}";

            renderer.SetMaterial(ChunkMaterial);
        }
    }
}