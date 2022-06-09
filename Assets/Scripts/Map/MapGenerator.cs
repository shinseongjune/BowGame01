using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject groundPrefab;

    public GridManager gridManager;

    public float[,] heightMap;

    public void GenerateMap()
    {
        int width = GridManager.GRID_X;
        int height = GridManager.GRID_Y;
        float size = GridManager.TILE_SIZE;

        heightMap = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                heightMap[x, y] = Mathf.PerlinNoise(x * size, y * size);
                print("(" + x + "," + y + ") = " + heightMap[x, y]);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject ground = Instantiate(groundPrefab, new Vector3(x * size, Mathf.RoundToInt(heightMap[x,z] * 5), z * size), Quaternion.identity);
                ground.transform.parent = transform;
            }
        }
    }
}
