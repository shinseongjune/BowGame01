using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject groundPrefab;
    public GameObject waterPrefab;
    public GameObject rockPrefab;

    public GridManager gridManager;

    public MapPiece[,] heightMap;

    public List<MapPiece> grounds = new();
    public List<MapPiece> hills = new();

    public const float OFFSET = 2f;

    private void Start()
    {
        GenerateMap();

        gridManager.GenerateGrid();
    }

    public void GenerateMap()
    {
        int width = GridManager.GRID_X;
        int height = GridManager.GRID_Y;
        float size = GridManager.TILE_SIZE;

        heightMap = new MapPiece[width, height];

        //√ ±‚»≠
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                //heightMap[x, y] = Mathf.PerlinNoise(x / size * OFFSET, y / size * OFFSET);
                //heightMap[x, y] = Mathf.RoundToInt(heightMap[x, y] * 2) * 5;

                heightMap[x, z] = new MapPiece();
                heightMap[x, z].x = x;
                heightMap[x, z].y = 0;
                heightMap[x, z].z = z;
                grounds.Add(heightMap[x, z]);
            }
        }

        //∑£¥˝ æ¥ˆ
        int hillSize = 1;
        while (hillSize < 20000)
        {
            int hillX = Random.Range(0, width);
            int hillZ = Random.Range(0, height);
            if (heightMap[hillX, hillZ].y == 1)
            {
                continue;
            }
            int[,] flags = new int[width, height];
            int chance = 80;

            heightMap[hillX, hillZ].y = 1;
            grounds.Remove(heightMap[hillX, hillZ]);
            hills.Add(heightMap[hillX, hillZ]);

            Queue<MapPiece> checkNow = new Queue<MapPiece>();
            Queue<MapPiece> checkNext = new Queue<MapPiece>();

            checkNow.Enqueue(heightMap[hillX, hillZ]);

            while (checkNow.Count > 0)
            {
                MapPiece piece = checkNow.Dequeue();

                for (int x = piece.x - 1; x <= piece.x + 1; x++)
                {
                    for (int z = piece.z - 1; z <= piece.z + 1; z++)
                    {
                        if (x == piece.x && z == piece.z)
                        {
                            continue;
                        }
                        else if (x < 0 || z < 0 || x >= width || z >= height)
                        {
                            continue;
                        }
                        else if (flags[x, z] == 1)
                        {
                            continue;
                        }
                        else
                        {
                            if (Random.Range(0, 100) <= chance)
                            {
                                heightMap[x, z].y = 1;
                                checkNext.Enqueue(heightMap[x, z]);
                                chance--;
                                hillSize++;
                                flags[x, z] = 1;
                                grounds.Remove(heightMap[x, z]);
                                hills.Add(heightMap[x, z]);
                            }
                        }
                    }
                }
                if (checkNow.Count == 0)
                {
                    var temp = checkNow;
                    checkNow = checkNext;
                    checkNext = temp;
                }
            }
        }

        SmoothingMap();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject prefab;
                if (heightMap[x,z].y == 0)
                {
                    prefab = groundPrefab;
                }
                else
                {
                    prefab = rockPrefab;
                }

                GameObject mapPiece = Instantiate(prefab, new Vector3(x * size + size / 2, heightMap[x,z].y * size, z * size + size / 2), Quaternion.identity);
                mapPiece.transform.SetParent(transform, false);
            }
        }
    }

    public void SmoothingMap()
    {
        int width = GridManager.GRID_X;
        int height = GridManager.GRID_Y;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                MapPiece piece = heightMap[x, z];
                int hillCount = 0;
                for (int offsetX = x - 1; offsetX <= x + 1; offsetX++)
                {
                    for (int offsetZ = z - 1; offsetZ <= z + 1; offsetZ++)
                    {
                        if (offsetX == x && offsetZ == z)
                        {
                            continue;
                        }
                        else if (offsetX < 0 || offsetZ < 0 || offsetX >= width || offsetZ >= height)
                        {
                            hillCount++;
                        }
                        else
                        {
                            if (heightMap[offsetX, offsetZ].y == 1)
                            {
                                hillCount++;
                            }
                        }
                    }
                }

                if (hillCount > 4)
                {
                    heightMap[x, z].y = 1;
                }
                else if (hillCount < 4)
                {
                    heightMap[x, z].y = 0;
                }
            }
        }
    }
}
