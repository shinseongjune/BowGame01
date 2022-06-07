using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Tile[,] grid = new Tile[GRID_X, GRID_Y];
    public TileLine[,] horizontalLines = new TileLine[GRID_X, GRID_Y + 1];
    public TileLine[,] verticalLines = new TileLine[GRID_X + 1, GRID_Y];

    public const float TILE_SIZE = 4.4f;
    public const int GRID_X = 16;
    public const int GRID_Y = 16;

    //TODO: delete this
    public bool isInitialized = false;
    //===============

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

        //TODO: delete this
        isInitialized = true;
        //================
    }

    private void OnDrawGizmos()
    {
        if (!isInitialized)
        {
            return;
        }

        GUIStyle style = new();
        style.fontStyle = FontStyle.Normal;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.red;

        for (int y = 0; y < GRID_Y; y++)
        {
            for (int x = 0; x < GRID_X; x++)
            {
                Vector3 pos = grid[x, y].position;
                string str = "(" + x + "," + y + ")";
                Handles.Label(pos, str, style);
            }
        }

        for (int y = 0; y < GRID_Y + 1; y++)
        {
            for (int x = 0; x < GRID_X; x++)
            {
                Vector3 pos = horizontalLines[x, y].position;
                Vector3 from = new(pos.x - TILE_SIZE / 2, 0, pos.z);
                Vector3 to = new(pos.x + TILE_SIZE / 2, 0, pos.z);
                Gizmos.DrawLine(from, to);
            }
        }

        for (int y = 0; y < GRID_Y; y++)
        {
            for (int x = 0; x < GRID_X + 1; x++)
            {
                Vector3 pos = verticalLines[x, y].position;
                Vector3 from = new(pos.x, 0, pos.z - TILE_SIZE / 2);
                Vector3 to = new(pos.x, 0, pos.z + TILE_SIZE / 2);
                Gizmos.DrawLine(from, to);
            }
        }
    }
}
