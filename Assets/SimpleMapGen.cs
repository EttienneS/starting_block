using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.StrategyCamera;
using System;
using UnityEngine;

public class SimpleMapGen : LocatableMonoBehavior
{
    public Material ChunkMaterial;

    [Range(5, 30)]
    public int ChunksToRender = 30;

    private Cell[,] map;

    public override void Initialize()
    {
        GenerateMap();

        MakeCellMagentaOnClick();

        ConfigureMapBounds();
    }

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

            Locate<ChunkManager>().RenderCells(map);
        }
    }

    private void MakeCellMagentaOnClick()
    {
        CellEventManager.OnCellClicked += (cell) =>
        {
            cell.Color = Color.magenta;
            Locate<ChunkManager>().GetRendererForCell(cell).GenerateMesh();
        };
    }

    private void ConfigureMapBounds()
    {
        var max = GetMapSize();
        var camera = Locate<CameraController>();
        camera.ConfigureBounds(0, max, 0, max + 50);
        camera.MoveToWorldCenter();
    }

    private void GenerateMap()
    {
        GetLocator().Unregister<ChunkManager>();
        GetLocator().Register(ChunkManager.CreateChunkManager(ChunkMaterial));
        RegenerateMap();
    }

    private int GetMapSize()
    {
        return Constants.ChunkSize * ChunksToRender;
    }
}