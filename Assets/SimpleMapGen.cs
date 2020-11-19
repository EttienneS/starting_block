using Assets.Helpers;
using Assets.Map;
using System;
using UnityEngine;

public class SimpleMapGen : MonoBehaviour
{
    public Material ChunkMaterial;

    private readonly int _mapSize = Constants.ChunkSize * 10;
    private ChunkManager _chunkManager;
    private float _delta;
    private ChunkCell[,] map;

    public void RegenerateMap()
    {
        map = new ChunkCell[_mapSize, _mapSize];

        var noise = new FastNoiseLite(Guid.NewGuid().GetHashCode());
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFrequency(0.01f);
        noise.SetFractalType(FastNoiseLite.FractalType.FBm);

        var height = noise.GetNoiseMap(_mapSize);
        for (var x = 0; x < _mapSize; x++)
        {
            for (var z = 0; z < _mapSize; z++)
            {
                var cellHeight = height[x, z] * 20f;

                if (cellHeight <= 0)
                {
                    cellHeight = 0;
                }

                Color color;
                if (cellHeight > 9)
                {
                    color = Color.white;
                }
                else if (cellHeight > 7)
                {
                    color = Color.grey;
                }
                else if (cellHeight > 5)
                {
                    color = ColorExtensions.GetColorFromHex("2d6a4f");
                }
                else if (cellHeight > 2)
                {
                    color = ColorExtensions.GetColorFromHex("52b788");
                }
                else if (cellHeight > 0)
                {
                    color = Color.yellow;
                }
                else
                {
                    color = Color.blue;
                }

                map[x, z] = new ChunkCell(x, z, cellHeight, color);
            }
        }

        _chunkManager.DestroyChunks();
        _chunkManager.RenderCells(map);
    }

    private void Start()
    {
        _chunkManager = ChunkManager.CreateChunkManager(ChunkMaterial);
        RegenerateMap();
        Camera.main.transform.position = new Vector3(_mapSize / 2, _mapSize / 2, -10);
    }

    private void Update()
    {
        _delta += Time.deltaTime;

        if (_delta > 2.5f)
        {
            _delta = 0;
            //RegenerateMap();
        }
    }
}