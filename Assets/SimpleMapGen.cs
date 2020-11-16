using Assets.Helpers;
using Assets.Map;
using UnityEngine;

public class SimpleMapGen : MonoBehaviour
{
    private ChunkManager _chunkManager;
    public Material ChunkMaterial;

    private int mapsize = Constants.ChunkSize * 5;
    private ChunkCell[,] map;

    private void Start()
    {
        _chunkManager = ChunkManager.CreateChunkManager(ChunkMaterial);

        map = new ChunkCell[mapsize, mapsize];
        for (var x = 0; x < mapsize; x++)
        {
            for (var z = 0; z < mapsize; z++)
            {
                map[x, z] = new ChunkCell(x, z, Random.Range(-1f, 1f), ColorExtensions.GetRandomColor());
            }
        }

        _chunkManager.RenderCells(map);
    }

    private void Update()
    {
        delta -= Time.deltaTime;

        if (delta <= 0)
        {
            delta = 5f;
            RegenerateMap();
        }
    }

    private void RegenerateMap()
    {
        for (var x = 0; x < mapsize; x++)
        {
            for (var z = 0; z < mapsize; z++)
            {
                map[x, z].Color = ColorExtensions.GetRandomColor();
            }
        }
        _chunkManager.RefreshChunks();
    }

    float delta = 5f;
}