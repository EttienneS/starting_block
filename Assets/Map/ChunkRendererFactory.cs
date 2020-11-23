using UnityEngine;

namespace Assets.Map
{
    public class ChunkRendererFactory
    {
        private readonly Camera _camera;

        public ChunkRendererFactory(Camera camera)
        {
            _camera = camera;
        }

        public ChunkRenderer CreateChunkRenderer(int x, int z, Cell[,] cells)
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();

            var chunkRenderer = gameObject.AddComponent<ChunkRenderer>();
            chunkRenderer.X = x;
            chunkRenderer.Z = z;
            chunkRenderer.SetCells(cells);
            chunkRenderer.SetCamera(_camera);
            return chunkRenderer;
        }
    }
}