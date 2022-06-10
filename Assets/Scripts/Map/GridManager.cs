using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Tile[,] grid = new Tile[GRID_X, GRID_Y];
    public TileLine[,] horizontalLines = new TileLine[GRID_X, GRID_Y + 1];
    public TileLine[,] verticalLines = new TileLine[GRID_X + 1, GRID_Y];

    public GameObject buildingGuideLinePrefab;

    public const float TILE_SIZE = 4.4f;
    public const int GRID_X = 256;
    public const int GRID_Y = 256;

    private void Start()
    {
        for (int y = 0; y < GRID_Y + 1; y++)
        {
            for (int x = 0; x < GRID_X; x++)
            {
                TileLine line = new(new Vector3((TILE_SIZE * x) + (TILE_SIZE / 2), 0, TILE_SIZE * y), true);
                horizontalLines[x, y] = line;
            }
        }

        for (int y = 0; y < GRID_Y; y++)
        {
            for (int x = 0; x < GRID_X + 1; x++)
            {
                TileLine line = new(new Vector3(TILE_SIZE * x, 0, (TILE_SIZE * y) + (TILE_SIZE / 2)), false);
                verticalLines[x, y] = line;
            }
        }

        for (int y = 0; y < GRID_Y; y++)
        {
            for (int x = 0; x < GRID_X; x++)
            {
                grid[x, y] = new Tile(TILE_SIZE, x, y, horizontalLines[x, y + 1], horizontalLines[x, y], verticalLines[x, y], verticalLines[x + 1, y]);
            }
        }

        foreach (TileLine line in horizontalLines)
        {
            GameObject go = Instantiate(buildingGuideLinePrefab);
            go.transform.SetParent(transform);
            LineRenderer renderer = go.GetComponent<LineRenderer>();
            Vector3 from = new(line.position.x - TILE_SIZE / 2, 0.1f, line.position.z);
            Vector3 to = new(line.position.x + TILE_SIZE / 2, 0.1f, line.position.z);
            renderer.SetPosition(0, from);
            renderer.SetPosition(1, to);
            go.SetActive(false);
        }

        foreach (TileLine line in verticalLines)
        {
            GameObject go = Instantiate(buildingGuideLinePrefab);
            go.transform.SetParent(transform);
            LineRenderer renderer = go.GetComponent<LineRenderer>();
            Vector3 from = new(line.position.x, 0.1f, line.position.z - TILE_SIZE / 2);
            Vector3 to = new(line.position.x, 0.1f, line.position.z + TILE_SIZE / 2);
            renderer.SetPosition(0, from);
            renderer.SetPosition(1, to);
            go.SetActive(false);
        }
    }
}
