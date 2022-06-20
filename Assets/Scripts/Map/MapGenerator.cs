using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    class Room
    {
        public List<MapPiece> mapPieces = new List<MapPiece>();
        public List<MapPiece> edges = new List<MapPiece>();

        public List<Room> connectedRooms = new List<Room>();

        public bool IsConnected(Room room)
        {
            return connectedRooms.Contains(room);
        }
    }

    class StairPoint
    {
        public int x;
        public int z;
        public MapPiece.Direction direction = MapPiece.Direction.None;
    }

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
    public const int GRID_X = 30;
    public const int GRID_Y = 30;

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

        //List<Room> rooms = GetRooms();

        //List<StairPoint> points = MakeStairsPositions(rooms);

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

        //foreach(StairPoint point in points)
        //{
        //    float angle = 0f;
        //
        //    switch (point.direction)
        //    {
        //        case MapPiece.Direction.North:
        //            angle = 0f;
        //            break;
        //        case MapPiece.Direction.South:
        //            angle = 180f;
        //            break;
        //        case MapPiece.Direction.West:
        //            angle = 270f;
        //            break;
        //        case MapPiece.Direction.East:
        //            angle = 90f;
        //            break;
        //    }
        //
        //    GameObject stairs = Instantiate(stairsPrefab, new Vector3(point.x * TILE_XZ + TILE_XZ / 2, TILE_HEIGHT, point.z * TILE_XZ + TILE_XZ / 2), Quaternion.AngleAxis(angle, Vector3.up));
        //    stairs.transform.SetParent(transform, false);
        //}

        NavMeshSurface navMeshSurface = transform.GetChild(0).GetComponent<NavMeshSurface>();
        if (navMeshSurface != null)
        {
            //navMeshSurface.BuildNavMesh();
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

    private List<StairPoint> MakeStairsPositions(List<Room> rooms)
    {
        foreach (Room room in rooms)
        {
            if (room.mapPieces[0].y == 0)
            {
                continue;
            }
            foreach(MapPiece piece in room.mapPieces)
            {
                int y = piece.y;

                if (piece.x > 0 && heightMap[piece.x - 1, piece.z].y != y) //øﬁ¬ 
                {
                    piece.direction |= MapPiece.Direction.West;
                }

                if (piece.x < GRID_X - 1 && heightMap[piece.x + 1, piece.z].y != y) //ø¿∏•¬ 
                {
                    piece.direction |= MapPiece.Direction.East;
                }

                if (piece.z < GRID_Y - 1 && heightMap[piece.x, piece.z + 1].y != y) //¿ß
                {
                    piece.direction |= MapPiece.Direction.North;
                }

                if (piece.z > 0 && heightMap[piece.x, piece.z - 1].y != y) //æ∆∑°
                {
                    piece.direction |= MapPiece.Direction.South;
                }

                if (piece.direction != MapPiece.Direction.None)
                {
                    room.edges.Add(piece);
                }
            }
        }

        List<StairPoint> points = new();

        foreach (Room room in rooms)
        {
            if (room.mapPieces[0].y == 0)
            {
                continue;
            }
            bool isMakingPoint = true;
            while(isMakingPoint)
            {
                int index = Random.Range(0, room.edges.Count);
                int rand;

                MapPiece piece = room.edges[index];

                switch (piece.direction)
                {
                    case MapPiece.Direction.NorthWest:
                        rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            if (piece.z + 2 < GRID_Y && heightMap[piece.x, piece.z +2].y != piece.y)
                            {
                                StairPoint point = new();
                                point.x = piece.x;
                                point.z = piece.z + 1;
                                point.direction = MapPiece.Direction.North;
                                points.Add(point);
                                isMakingPoint = false;
                            }
                        }
                        else
                        {
                            if (piece.x - 2 >= 0 && heightMap[piece.x - 1, piece.z].y != piece.y)
                            {
                                StairPoint point = new();
                                point.x = piece.x - 1;
                                point.z = piece.z;
                                point.direction = MapPiece.Direction.West;
                                points.Add(point);
                                isMakingPoint = false;
                            }
                        }
                        break;
                    case MapPiece.Direction.SouthWest:
                        rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            if (piece.z - 2 >= 0 && heightMap[piece.x, piece.z - 2].y != piece.y)
                            {
                                StairPoint point = new();
                                point.x = piece.x;
                                point.z = piece.z - 1;
                                point.direction = MapPiece.Direction.South;
                                points.Add(point);
                                isMakingPoint = false;
                            }
                        }
                        else
                        {
                            if (piece.x - 2 >= 0 && heightMap[piece.x - 1, piece.z].y != piece.y)
                            {
                                StairPoint point = new();
                                point.x = piece.x - 1;
                                point.z = piece.z;
                                point.direction = MapPiece.Direction.West;
                                points.Add(point);
                                isMakingPoint = false;
                            }
                        }
                        break;
                    case MapPiece.Direction.NorthEast:
                        rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            if (piece.z + 2 < GRID_Y && heightMap[piece.x, piece.z + 2].y != piece.y)
                            {
                                StairPoint point = new();
                                point.x = piece.x;
                                point.z = piece.z + 1;
                                point.direction = MapPiece.Direction.North;
                                points.Add(point);
                                isMakingPoint = false;
                            }
                        }
                        else
                        {
                            if (piece.x + 2 < GRID_X && heightMap[piece.x + 2, piece.z].y != piece.y)
                            {
                                StairPoint point = new();
                                point.x = piece.x + 1;
                                point.z = piece.z;
                                point.direction = MapPiece.Direction.East;
                                points.Add(point);
                                isMakingPoint = false;
                            }
                        }
                        break;
                    case MapPiece.Direction.SouthEast:
                        rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            if (piece.z - 2 >= 0 && heightMap[piece.x, piece.z - 2].y != piece.y)
                            {
                                StairPoint point = new();
                                point.x = piece.x;
                                point.z = piece.z - 1;
                                point.direction = MapPiece.Direction.South;
                                points.Add(point);
                                isMakingPoint = false;
                            }
                        }
                        else
                        {
                            if (piece.x + 2 < GRID_X && heightMap[piece.x + 2, piece.z].y != piece.y)
                            {
                                StairPoint point = new();
                                point.x = piece.x + 1;
                                point.z = piece.z;
                                point.direction = MapPiece.Direction.East;
                                points.Add(point);
                                isMakingPoint = false;
                            }
                        }
                        break;
                    case MapPiece.Direction.North:
                        if (piece.z + 2 < GRID_Y && heightMap[piece.x, piece.z + 2].y != piece.y)
                        {
                            StairPoint point = new();
                            point.x = piece.x;
                            point.z = piece.z + 1;
                            point.direction = MapPiece.Direction.North;
                            points.Add(point);
                            isMakingPoint = false;
                        }
                        break;
                    case MapPiece.Direction.South:
                        if (piece.z - 2 >= 0 && heightMap[piece.x, piece.z - 2].y != piece.y)
                        {
                            StairPoint point = new();
                            point.x = piece.x;
                            point.z = piece.z - 1;
                            point.direction = MapPiece.Direction.South;
                            points.Add(point);
                            isMakingPoint = false;
                        }
                        break;
                    case MapPiece.Direction.West:
                        if (piece.x - 2 >= 0 && heightMap[piece.x - 1, piece.z].y != piece.y)
                        {
                            StairPoint point = new();
                            point.x = piece.x - 1;
                            point.z = piece.z;
                            point.direction = MapPiece.Direction.West;
                            points.Add(point);
                            isMakingPoint = false;
                        }
                        break;
                    case MapPiece.Direction.East:
                        if (piece.x + 2 < GRID_X && heightMap[piece.x + 2, piece.z].y != piece.y)
                        {
                            StairPoint point = new();
                            point.x = piece.x + 1;
                            point.z = piece.z;
                            point.direction = MapPiece.Direction.East;
                            points.Add(point);
                            isMakingPoint = false;
                        }
                        break;
                }
            }
        }

        return points;
    }

    private List<Room> GetRooms()
    {
        List<Room> rooms = new();

        int[,] flags = new int[GRID_X, GRID_Y];

        for (int x = 0; x < GRID_X; x++)
        {
            for (int z = 0; z < GRID_Y; z++)
            {
                if (flags[x,z] == 0)
                {
                    flags[x, z] = 1;

                    Room room = new();

                    Queue<MapPiece> checkNow = new();
                    Queue<MapPiece> checkNext = new();

                    checkNow.Enqueue(heightMap[x, z]);

                    while(checkNow.Count > 0)
                    {
                        MapPiece piece = checkNow.Dequeue();
                        int y = piece.y;
                        room.mapPieces.Add(piece);

                        for (int offsetX = piece.x - 1; offsetX <= piece.x + 1; offsetX++)
                        {
                            for (int offsetZ = piece.z - 1; offsetZ <= piece.z + 1; offsetZ++)
                            {
                                if (offsetX < 0 || offsetZ < 0 || offsetX >= GRID_X || offsetZ >= GRID_Y)
                                {
                                    continue;
                                }
                                if (offsetX == piece.x && offsetZ == piece.z)
                                {
                                    continue;
                                }
                                if (flags[offsetX, offsetZ] == 1)
                                {
                                    continue;
                                }

                                if (offsetX == piece.x || offsetZ == piece.z)
                                {
                                    flags[offsetX, offsetZ] = 1;
                                    
                                    if (heightMap[offsetX, offsetZ].y == y)
                                    {
                                        checkNext.Enqueue(heightMap[offsetX, offsetZ]);
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

                    rooms.Add(room);
                }
            }
        }

        return rooms;
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
