using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    public class Room
    {
        public List<MapPiece> mapPieces = new List<MapPiece>();
        public Dictionary<Room, List<MapPiece>> edges = new();

        public HashSet<Room> connectedRooms = new HashSet<Room>();

        public bool isMainRoom = false;

        public void ConnectRoom(Room room)
        {
            if (!connectedRooms.Contains(room))
            {
                connectedRooms.Add(room);
            }
            foreach (Room r in room.connectedRooms)
            {
                connectedRooms.Add(r);
            }
        }

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

    public Transform player;

    public GameObject groundPrefab;
    public GameObject stairsPrefab;
    public GameObject rockPrefab;
    public GameObject endLinePrefab;

    public GridManager gridManager;

    public MapPiece[,] heightMap;

    public List<MapPiece> grounds = new();
    public List<MapPiece> hills = new();

    public const float OFFSET = 2f;

    public static float TILE_XZ;
    public static float TILE_HEIGHT;
    public const int GRID_X = 100;
    public const int GRID_Y = 100;
    public const float ENDLINE_THICKNESS = 20f;

    List<Room> rooms;

    private void Start()
    {
        TILE_XZ = groundPrefab.transform.localScale.x;
        TILE_HEIGHT = groundPrefab.transform.localScale.y;
    }

    public void GenerateMap()
    {
        heightMap = new MapPiece[GRID_X, GRID_Y];

        //초기화
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

        //랜덤 언덕
        int hillSize = 1;

        while (hillSize < GRID_X * GRID_Y * 0.35f)
        {
            int randomIndex = Random.Range(0, grounds.Count);
            MapPiece hillPiece = grounds[randomIndex];

            int[,] flags = new int[GRID_X, GRID_Y];
            int chance = 80;

            hillPiece.y = 1;
            grounds.Remove(hillPiece);
            hills.Add(hillPiece);

            Queue<MapPiece> checkNow = new Queue<MapPiece>();
            Queue<MapPiece> checkNext = new Queue<MapPiece>();

            checkNow.Enqueue(hillPiece);

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
                        if (x < 0 || z < 0 || x >= GRID_X || z >= GRID_Y)
                        {
                            continue;
                        }
                        if (flags[x, z] == 1)
                        {
                            continue;
                        }

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
                if (checkNow.Count == 0)
                {
                    var temp = checkNow;
                    checkNow = checkNext;
                    checkNext = temp;
                }
            }
        }
        //랜덤 언덕 끝

        SmoothingMap();

        rooms = GetRooms();
        GetEdgeTiles();
        List<StairPoint> points = MakeStairsPositions(rooms);

        //지형 시작
        for (int x = 0; x < GRID_X; x++)
        {
            for (int z = 0; z < GRID_Y; z++)
            {
                GameObject prefab;
                if (heightMap[x, z].y == 0)
                {
                    prefab = groundPrefab;
                }
                else
                {
                    prefab = rockPrefab;
                }
        
                GameObject mapPiece = Instantiate(prefab, new Vector3(x * TILE_XZ + TILE_XZ / 2, heightMap[x, z].y * TILE_HEIGHT, z * TILE_XZ + TILE_XZ / 2), Quaternion.identity);
                mapPiece.transform.SetParent(transform, false);
            }
        }

        //테두리 시작
        GameObject endLineCollider1 = Instantiate(endLinePrefab, new Vector3(TILE_XZ * GRID_X / 2f, 5f, -ENDLINE_THICKNESS / 2), Quaternion.identity);
        endLineCollider1.transform.localScale = new Vector3(GRID_X * TILE_XZ + ENDLINE_THICKNESS * 2, 10f, ENDLINE_THICKNESS);
        endLineCollider1.transform.SetParent(transform, false);

        GameObject endLineCollider2 = Instantiate(endLinePrefab, new Vector3(-ENDLINE_THICKNESS / 2, 5f, TILE_XZ * GRID_Y / 2f), Quaternion.identity);
        endLineCollider2.transform.localScale = new Vector3(ENDLINE_THICKNESS, 10f, GRID_Y * TILE_XZ);
        endLineCollider2.transform.SetParent(transform, false);

        GameObject endLineCollider3 = Instantiate(endLinePrefab, new Vector3(TILE_XZ * GRID_X + ENDLINE_THICKNESS / 2, 5f, TILE_XZ * GRID_Y / 2f), Quaternion.identity);
        endLineCollider3.transform.localScale = new Vector3(ENDLINE_THICKNESS, 10f, GRID_Y * TILE_XZ);
        endLineCollider3.transform.SetParent(transform, false);

        GameObject endLineCollider4 = Instantiate(endLinePrefab, new Vector3(TILE_XZ * GRID_X / 2f, 5f, TILE_XZ * GRID_Y + ENDLINE_THICKNESS / 2f), Quaternion.identity);
        endLineCollider4.transform.localScale = new Vector3(GRID_X * TILE_XZ + ENDLINE_THICKNESS * 2, 10f, ENDLINE_THICKNESS);
        endLineCollider4.transform.SetParent(transform, false);
        //테두리 끝
        //지형 끝

        //계단 시작
        foreach (StairPoint point in points)
        {
            float angle = 0f;
        
            switch (point.direction)
            {
                case MapPiece.Direction.North:
                    angle = 0f;
                    break;
                case MapPiece.Direction.South:
                    angle = 180f;
                    break;
                case MapPiece.Direction.West:
                    angle = 270f;
                    break;
                case MapPiece.Direction.East:
                    angle = 90f;
                    break;
            }
        
            GameObject stairs = Instantiate(stairsPrefab, new Vector3(point.x * TILE_XZ + TILE_XZ / 2, TILE_HEIGHT / 2, point.z * TILE_XZ + TILE_XZ / 2), Quaternion.AngleAxis(angle, Vector3.up));
            stairs.transform.SetParent(transform, false);
        }
        //계단 끝

        //플레이어 배치 시작
        while(true)
        {
            int x = Random.Range(0, GRID_X);
            int z = Random.Range(0, GRID_Y);
            
            if (heightMap[x, z].y == 1)
            {
                continue;
            }

            player.transform.position = new Vector3(x * TILE_XZ + TILE_XZ / 2, 3f, z * TILE_XZ + TILE_XZ / 2);
            break;
        }
        //플레이어 배치 끝

        //내비게이션 시작
        NavMeshSurface navMeshSurface = transform.GetChild(0).GetComponent<NavMeshSurface>();
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        //내비게이션 끝
    }

    private List<StairPoint> MakeStairsPositions(List<Room> rooms)
    {
        Room mainRoom = rooms[0];
        int maxSize = mainRoom.mapPieces.Count;
        foreach (Room room in rooms)
        {
            if (maxSize < room.mapPieces.Count)
            {
                maxSize = room.mapPieces.Count;
                mainRoom = room;
            }
        }

        List<StairPoint> points = new();

        foreach (Room room in rooms)
        {
            if (room.mapPieces[0].y == 0)
            {
                continue;
            }

            foreach (Room room2 in rooms)
            {
                if (room2.mapPieces.Contains(heightMap[0, 0]))
                {
                    continue;
                }
                if (room == room2)
                {
                    continue;
                }

                List<MapPiece> edges = room.edges[room2];

                int stairsCount = Mathf.Max(edges.Count / 3, 1);

                for (int i = 0; i < stairsCount; i++)
                {
                    if (edges.Count <= 0)
                    {
                        break;
                    }

                    int index = Random.Range(0, edges.Count);
                    MapPiece piece = edges[index];

                    if ((piece.direction[room2] & MapPiece.Direction.North) > 0)
                    {
                        if (piece.z + 2 < GRID_Y && heightMap[piece.x, piece.z + 2].y != piece.y)
                        {
                            bool isAvailable = true;
                            foreach (StairPoint p in points)
                            {
                                if ((p.x == piece.x && p.z == piece.z + 1) || (p.x == piece.x && p.z == piece.z + 2) || (p.x == piece.x + 1 && p.z == piece.z + 1 && p.direction == MapPiece.Direction.West) || (p.x == piece.x - 1 && p.z == piece.z + 1 && p.direction == MapPiece.Direction.East))
                                {
                                    isAvailable = false;
                                    break;
                                }
                            }

                            if (isAvailable)
                            {
                                StairPoint point = new();
                                point.x = piece.x;
                                point.z = piece.z + 1;
                                point.direction = MapPiece.Direction.North;
                                points.Add(point);
                                Room connectedRoom = heightMap[point.x, point.z].room;
                                room.ConnectRoom(connectedRoom);
                                edges.RemoveAt(index);
                            }
                            else
                            {
                                edges.RemoveAt(index);
                                i--;
                            }
                        }
                        else
                        {
                            edges.RemoveAt(index);
                            i--;
                        }
                    }

                    if ((piece.direction[room2] & MapPiece.Direction.East) > 0)
                    {
                        if (piece.x + 2 < GRID_X && heightMap[piece.x + 2, piece.z].y != piece.y)
                        {
                            bool isAvailable = true;
                            foreach (StairPoint p in points)
                            {
                                if ((p.x == piece.x + 1 && p.z == piece.z) || (p.x == piece.x + 2 && p.z == piece.z) || (p.x == piece.x + 1 && p.z == piece.z - 1 && p.direction == MapPiece.Direction.North) || (p.x == piece.x + 1 && p.z == piece.z + 1 && p.direction == MapPiece.Direction.South))
                                {
                                    isAvailable = false;
                                    break;
                                }
                            }

                            if (isAvailable)
                            {
                                StairPoint point = new();
                                point.x = piece.x + 1;
                                point.z = piece.z;
                                point.direction = MapPiece.Direction.East;
                                points.Add(point);
                                Room connectedRoom = heightMap[point.x, point.z].room;
                                room.ConnectRoom(connectedRoom);
                                edges.RemoveAt(index);
                            }
                            else
                            {
                                edges.RemoveAt(index);
                                i--;
                            }
                        }
                        else
                        {
                            edges.RemoveAt(index);
                            i--;
                        }
                    }

                    if ((piece.direction[room2] & MapPiece.Direction.West) > 0)
                    {
                        if (piece.x - 2 >= 0 && heightMap[piece.x - 2, piece.z].y != piece.y)
                        {
                            bool isAvailable = true;
                            foreach (StairPoint p in points)
                            {
                                if ((p.x == piece.x - 1 && p.z == piece.z) || (p.x == piece.x - 2 && p.z == piece.z) || (p.x == piece.x - 1 && p.z == piece.z - 1 && p.direction == MapPiece.Direction.North) || (p.x == piece.x - 1 && p.z == piece.z + 1 && p.direction == MapPiece.Direction.South))
                                {
                                    isAvailable = false;
                                    break;
                                }
                            }

                            if (isAvailable)
                            {
                                StairPoint point = new();
                                point.x = piece.x - 1;
                                point.z = piece.z;
                                point.direction = MapPiece.Direction.West;
                                points.Add(point);
                                Room connectedRoom = heightMap[point.x, point.z].room;
                                room.ConnectRoom(connectedRoom);
                                edges.RemoveAt(index);
                            }
                            else
                            {
                                edges.RemoveAt(index);
                                i--;
                            }
                        }
                        else
                        {
                            edges.RemoveAt(index);
                            i--;
                        }
                    }

                    if ((piece.direction[room2] & MapPiece.Direction.South) > 0)
                    {
                        if (piece.z - 2 >= 0 && heightMap[piece.x, piece.z - 2].y != piece.y)
                        {
                            bool isAvailable = true;
                            foreach (StairPoint p in points)
                            {
                                if ((p.x == piece.x && p.z == piece.z - 1) || (p.x == piece.x && p.z == piece.z - 2) || (p.x == piece.x + 1 && p.z == piece.z - 1 && p.direction == MapPiece.Direction.West) || (p.x == piece.x - 1 && p.z == piece.z - 1 && p.direction == MapPiece.Direction.East))
                                {
                                    isAvailable = false;
                                    break;
                                }
                            }

                            if (isAvailable)
                            {
                                StairPoint point = new();
                                point.x = piece.x;
                                point.z = piece.z - 1;
                                point.direction = MapPiece.Direction.South;
                                points.Add(point);
                                Room connectedRoom = heightMap[point.x, point.z].room;
                                room.ConnectRoom(connectedRoom);
                                edges.RemoveAt(index);
                            }
                            else
                            {
                                edges.RemoveAt(index);
                                i--;
                            }
                        }
                        else
                        {
                            edges.RemoveAt(index);
                            i--;
                        }
                    }
                }
            }
        }

        return points;
    }

    private void GetEdgeTiles()
    {
        foreach (Room room in rooms)
        {
            foreach (Room room2 in rooms)
            {
                if (room == room2)
                {
                    continue;
                }

                room.edges.Add(room2, new());
            }
        }

        foreach (Room room in rooms)
        {
            if (room.mapPieces[0].y == 0)
            {
                continue;
            }

            foreach (MapPiece piece in room.mapPieces)
            {
                if (piece.x > 0 && heightMap[piece.x - 1, piece.z].room != piece.room) //왼쪽
                {
                    MapPiece nearPiece = heightMap[piece.x - 1, piece.z];
                    if (!piece.direction.ContainsKey(nearPiece.room))
                    {
                        piece.direction.Add(nearPiece.room, MapPiece.Direction.West);
                        room.edges[nearPiece.room].Add(piece);
                    }
                }

                if (piece.x < GRID_X - 1 && heightMap[piece.x + 1, piece.z].room != piece.room) //오른쪽
                {
                    MapPiece nearPiece = heightMap[piece.x + 1, piece.z];
                    if (!piece.direction.ContainsKey(nearPiece.room))
                    {
                        piece.direction.Add(nearPiece.room, MapPiece.Direction.East);
                        room.edges[nearPiece.room].Add(piece);
                    }
                }

                if (piece.z < GRID_Y - 1 && heightMap[piece.x, piece.z + 1].room != piece.room) //위
                {
                    MapPiece nearPiece = heightMap[piece.x, piece.z + 1];
                    if (!piece.direction.ContainsKey(nearPiece.room))
                    {
                        piece.direction.Add(nearPiece.room, MapPiece.Direction.North);
                        room.edges[nearPiece.room].Add(piece);
                    }
                }

                if (piece.z > 0 && heightMap[piece.x, piece.z - 1].room != piece.room) //아래
                {
                    MapPiece nearPiece = heightMap[piece.x, piece.z - 1];
                    if (!piece.direction.ContainsKey(nearPiece.room))
                    {
                        piece.direction.Add(nearPiece.room, MapPiece.Direction.South);
                        room.edges[nearPiece.room].Add(piece);
                    }
                }
            }
        }
    }

    private List<Room> GetRooms()
    {
        List<Room> rooms = new();


        for (int x = 0; x < GRID_X; x++)
        {
            for (int z = 0; z < GRID_Y; z++)
            {
                if (heightMap[x, z].room == null)
                {
                    Room room = new();

                    Queue<MapPiece> checkNow = new();
                    Queue<MapPiece> checkNext = new();

                    checkNow.Enqueue(heightMap[x, z]);

                    while (checkNow.Count > 0)
                    {
                        MapPiece piece = checkNow.Dequeue();
                        int y = piece.y;
                        room.mapPieces.Add(piece);
                        piece.room = room;

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
                                if (heightMap[offsetX, offsetZ].room != null)
                                {
                                    continue;
                                }

                                if (offsetX == piece.x || offsetZ == piece.z)
                                {
                                    if (heightMap[offsetX, offsetZ].y == y && !checkNow.Contains(heightMap[offsetX, offsetZ]) && !checkNext.Contains(heightMap[offsetX, offsetZ]))
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
        int width = GRID_X;
        int height = GRID_Y;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                int hillCount = 0;
                for (int offsetX = x - 1; offsetX <= x + 1; offsetX++)
                {
                    for (int offsetZ = z - 1; offsetZ <= z + 1; offsetZ++)
                    {
                        if (offsetX < 0 || offsetZ < 0 || offsetX >= width || offsetZ >= height)
                        {
                            hillCount++;
                            continue;
                        }
                        if (offsetX == x && offsetZ == z)
                        {
                            continue;
                        }
                        
                        if (heightMap[offsetX, offsetZ].y == 1)
                        {
                            hillCount++;
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
