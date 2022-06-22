using System;
using System.Collections.Generic;
using UnityEngine;

public class MapPiece
{
    [Flags]
    public enum Direction
    {
        None    = 0,
        North   = 1,
        South   = 2,
        West    = 4,
        East    = 8,
    }

    public int x;
    public int y;
    public int z;

    public Dictionary<MapGenerator.Room, Direction> direction = new();

    public MapGenerator.Room room = null;

    public Vector3 GetPosition(float tileXZ, float tileY)
    {
        return new Vector3((x * tileXZ) + tileXZ / 2, y * tileY, (z * tileXZ) + tileXZ / 2);
    }
}
