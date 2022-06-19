using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public MapGenerator mapGenerator;

    public Tile[,] grid = new Tile[GRID_X, GRID_Y];

    public List<TileLine> horizontalLines = new();
    public List<TileLine> verticalLines = new();

    public GameObject buildingGuideLinePrefab;

    public float TILE_XZ;
    public float TILE_HEIGHT;
    public const int GRID_X = 256;
    public const int GRID_Y = 256;

    private void Start()
    {
        TILE_XZ = MapGenerator.TILE_XZ;
        TILE_HEIGHT = MapGenerator.TILE_HEIGHT;
    }

    public void GenerateGrid()
    {
        //tile grid
        for (int z = 0; z < GRID_Y; z++)
        {
            for (int x = 0; x < GRID_X; x++)
            {
                float y;
                if (mapGenerator.heightMap[x,z].y == 0)
                {
                    y = TILE_HEIGHT / 2;
                }
                else
                {
                    y = TILE_HEIGHT * 1.5f;
                }
                Tile tile = new(x, y, z);
                grid[x, z] = tile;
            }
        }

        for (int z = 0; z < GRID_Y; z++)
        {
            for (int x = 0; x < GRID_X; x++)
            {
                Tile tile = grid[x, z];

                Vector3 tilePosition = new(tile.x * TILE_XZ + TILE_XZ / 2, tile.y, tile.z * TILE_XZ + TILE_XZ / 2);
                TileLine top = new(new(tilePosition.x, tilePosition.y + 0.3f, tilePosition.z + TILE_XZ / 2), true);
                TileLine right = new(new(tilePosition.x + TILE_XZ / 2, tilePosition.y + 0.3f, tilePosition.z), false);

                horizontalLines.Add(top);
                verticalLines.Add(right);

                if (tile.x == 0 || grid[x - 1, z].y != tile.y)
                {
                    TileLine left = new(new(tilePosition.x - TILE_XZ / 2, tilePosition.y + 0.3f, tilePosition.z), false);
                    verticalLines.Add(left);
                }
                if (tile.z == 0 || grid[x, z - 1].y != tile.y)
                {
                    TileLine bottom = new(new(tilePosition.x, tilePosition.y + 0.3f, tilePosition.z - TILE_XZ / 2), true);
                    horizontalLines.Add(bottom);
                }
            }
        }

        foreach(TileLine line in horizontalLines)
        {
            GameObject go = Instantiate(buildingGuideLinePrefab);
            LineRenderer renderer = go.GetComponent<LineRenderer>();
            Vector3 from = new(line.position.x - TILE_XZ / 2, line.position.y, line.position.z);
            Vector3 to = new(line.position.x + TILE_XZ / 2, line.position.y, line.position.z);
            renderer.SetPosition(0, from);
            renderer.SetPosition(1, to);

            if (Mathf.Approximately(line.position.y, TILE_HEIGHT / 2 + 0.3f))
            {
                go.transform.SetParent(transform.GetChild(0));
            }
            else
            {
                go.transform.SetParent(transform.GetChild(1));
            }
        }

        foreach (TileLine line in verticalLines)
        {
            GameObject go = Instantiate(buildingGuideLinePrefab);
            LineRenderer renderer = go.GetComponent<LineRenderer>();
            Vector3 from = new(line.position.x, line.position.y, line.position.z - TILE_XZ / 2);
            Vector3 to = new(line.position.x, line.position.y, line.position.z + TILE_XZ / 2);
            renderer.SetPosition(0, from);
            renderer.SetPosition(1, to);

            if (Mathf.Approximately(line.position.y, TILE_HEIGHT / 2 + 0.3f))
            {
                go.transform.SetParent(transform.GetChild(0));
            }
            else
            {
                go.transform.SetParent(transform.GetChild(1));
            }
        }

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
