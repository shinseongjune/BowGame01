using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public MapGenerator mapGenerator;

    public Tile[,] grid = new Tile[GRID_X, GRID_Y];

    public TileLine[,] groundHorizontalLines = new TileLine[GRID_X, GRID_Y + 1];
    public TileLine[,] hillHorizontalLines = new TileLine[GRID_X, GRID_Y + 1];
    public TileLine[,] groundVerticalLines = new TileLine[GRID_X + 1, GRID_Y];
    public TileLine[,] hillVerticalLines = new TileLine[GRID_X + 1, GRID_Y];

    public GameObject buildingGuideLinePrefab;

    public const float TILE_SIZE = 4.4f;
    public const int GRID_X = 256;
    public const int GRID_Y = 256;

    public void GenerateGrid()
    {
        MapPiece[,] heightMap = mapGenerator.heightMap;

        //tile grid
        for (int z = 0; z < GRID_Y; z++)
        {
            for (int x = 0; x < GRID_X; x++)
            {
                float y;
                if (mapGenerator.heightMap[x,z].y == 0)
                {
                    y = TILE_SIZE;
                }
                else
                {
                    y = TILE_SIZE * 2;
                }
                Tile tile = new(x, y, z);
                grid[x, z] = tile;
            }
        }

        //horizontal line generate
        for (int z = 0; z < GRID_Y + 1; z++)
        {
            for (int x = 0; x < GRID_X; x++)
            {
                TileLine line;

                if (heightMap[x, z].y == 0)
                {
                    line = new(new Vector3((TILE_SIZE * x) + TILE_SIZE / 2, TILE_SIZE, TILE_SIZE * z), true);
                    groundHorizontalLines[x, z] = line;
                }
                else
                {
                    line = new(new Vector3((TILE_SIZE * x) + TILE_SIZE / 2, TILE_SIZE * 2, TILE_SIZE * z), true);
                    hillHorizontalLines[x, z] = line;
                }
            }
        }

        //vertical line generate
        for (int z = 0; z < GRID_Y; z++)
        {
            for (int x = 0; x < GRID_X + 1; x++)
            {
                TileLine line;

                if (heightMap[x, z].y == 0)
                {
                    line = new(new Vector3(TILE_SIZE * x, TILE_SIZE, (TILE_SIZE * z) + TILE_SIZE / 2), false);
                    groundVerticalLines[x, z] = line;
                }
                else
                {
                    line = new(new Vector3(TILE_SIZE * x, TILE_SIZE * 2, (TILE_SIZE * z) + TILE_SIZE / 2), false);
                    hillVerticalLines[x, z] = line;
                }
            }
        }

        //horizontal line object generate
        foreach (TileLine line in groundHorizontalLines)
        {
            GameObject go = Instantiate(buildingGuideLinePrefab);
            go.transform.SetParent(transform);
            LineRenderer renderer = go.GetComponent<LineRenderer>();
            Vector3 from = new(line.position.x - TILE_SIZE / 2, TILE_SIZE, line.position.z);
            Vector3 to = new(line.position.x + TILE_SIZE / 2, TILE_SIZE, line.position.z);
            renderer.SetPosition(0, from);
            renderer.SetPosition(1, to);
            go.SetActive(false);
        }

        foreach (TileLine line in hillHorizontalLines)
        {
            GameObject go = Instantiate(buildingGuideLinePrefab);
            go.transform.SetParent(transform);
            LineRenderer renderer = go.GetComponent<LineRenderer>();
            Vector3 from = new(line.position.x - TILE_SIZE / 2, TILE_SIZE * 2, line.position.z);
            Vector3 to = new(line.position.x + TILE_SIZE / 2, TILE_SIZE * 2, line.position.z);
            renderer.SetPosition(0, from);
            renderer.SetPosition(1, to);
            go.SetActive(false);
        }

        //vertical line object generate
        foreach (TileLine line in groundVerticalLines)
        {
            GameObject go = Instantiate(buildingGuideLinePrefab);
            go.transform.SetParent(transform);
            LineRenderer renderer = go.GetComponent<LineRenderer>();
            Vector3 from = new(line.position.x, TILE_SIZE, line.position.z - TILE_SIZE / 2);
            Vector3 to = new(line.position.x, TILE_SIZE, line.position.z + TILE_SIZE / 2);
            renderer.SetPosition(0, from);
            renderer.SetPosition(1, to);
            go.SetActive(false);
        }

        foreach (TileLine line in hillVerticalLines)
        {
            GameObject go = Instantiate(buildingGuideLinePrefab);
            go.transform.SetParent(transform);
            LineRenderer renderer = go.GetComponent<LineRenderer>();
            Vector3 from = new(line.position.x, TILE_SIZE * 2, line.position.z - TILE_SIZE / 2);
            Vector3 to = new(line.position.x, TILE_SIZE * 2, line.position.z + TILE_SIZE / 2);
            renderer.SetPosition(0, from);
            renderer.SetPosition(1, to);
            go.SetActive(false);
        }
    }
}
