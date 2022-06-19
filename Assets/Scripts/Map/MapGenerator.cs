using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    public GameObject groundPrefab;
    public GameObject stairsPrefab;
    public GameObject rockPrefab;

    public GridManager gridManager;

    public MapPiece[,] heightMap;

    public List<MapPiece> grounds = new();
    public List<MapPiece> hills = new();

    public const float OFFSET = 2f;

    public static float TILE_XZ;
    public static float TILE_HEIGHT;
    public const int GRID_X = 256;
    public const int GRID_Y = 256;

    private void Start()
    {
        TILE_XZ = groundPrefab.transform.localScale.x;
        TILE_HEIGHT = groundPrefab.transform.localScale.y;
    }

    public void GenerateMap()
    {
        heightMap = new MapPiece[GRID_X, GRID_Y];

        //√ ±‚»≠
        for (int z = 0; z < GRID_Y; z++)
        {
            for (int x = 0; x < GRID_X; x++)
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
            int hillX = Random.Range(0, GRID_X);
            int hillZ = Random.Range(0, GRID_Y);
            if (heightMap[hillX, hillZ].y == 1)
            {
                continue;
            }
            int[,] flags = new int[GRID_X, GRID_Y];
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
                        else if (x < 0 || z < 0 || x >= GRID_X || z >= GRID_Y)
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

        for (int x = 0; x < GRID_X; x++)
        {
            for (int z = 0; z < GRID_Y; z++)
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

                GameObject mapPiece = Instantiate(prefab, new Vector3(x * TILE_XZ + TILE_XZ / 2, heightMap[x,z].y * TILE_HEIGHT, z * TILE_XZ + TILE_XZ / 2), Quaternion.identity);
                mapPiece.transform.SetParent(transform, false);
            }
        }

        NavMeshSurface navMeshSurface = transform.GetChild(0).GetComponent<NavMeshSurface>();
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        //foreach (Transform mapPiece in transform)
        //{
            //NavMeshSurface navMeshSurface = mapPiece.GetComponent<NavMeshSurface>();
            //if (navMeshSurface != null)
            //{
            //    navMeshSurface.BuildNavMesh();
            //}
        //}
    }

    public void SmoothingMap()
    {
        int width = GridManager.GRID_X;
        int height = GridManager.GRID_Y;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
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
