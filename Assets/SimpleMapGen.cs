using Assets.Helpers;
using Assets.Map;
using System;
using UnityEngine;

public class SimpleMapGen : MonoBehaviour
{
    public Material ChunkMaterial;

    [Range(5, 30)]
    public int ChunksToRender = 30;

    private ChunkManager _chunkManager;

    private float _delta;

    private Cell[,] map;

    public void RegenerateMap()
    {
        using (Instrumenter.Start())
        {
            var mapSize = GetMapSize();
            map = new Cell[mapSize, mapSize];

            var noise = new FastNoiseLite(Guid.NewGuid().GetHashCode());
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.01f);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);

            var height = noise.GetNoiseMap(mapSize);
            for (var x = 0; x < mapSize; x++)
            {
                for (var z = 0; z < mapSize; z++)
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

                    map[x, z] = new Cell(x, z, cellHeight, color);
                }
            }

            _chunkManager.RenderCells(map);
        }
    }

    private int GetMapSize()
    {
        return Constants.ChunkSize * ChunksToRender;
    }

    private void Start()
    {
        _chunkManager = ChunkManager.CreateChunkManager(ChunkMaterial);
        RegenerateMap();

        CellEventManager.OnCellClicked += (cell) =>
        {
            cell.Color = Color.magenta;
            _chunkManager.GetRendererForCell(cell).GenerateMesh();
        };
    }

    private void Update()
    {
        _delta += Time.deltaTime;

        if (_delta > 2.5f)
        {
            _delta = 0;
        }
    }
}