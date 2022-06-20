using System;
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
        NorthWest = North | West,
        SouthWest = South | West,
        NorthEast = North | East,
        SouthEast = South | East,
    }

    public int x;
    public int y;
    public int z;

    public Direction direction = Direction.None;

    public Vector3 GetPosition(float tileSize)
    {
        return new Vector3((x * tileSize) + tileSize / 2, (y * tileSize) + tileSize / 2, (z * tileSize) + tileSize / 2);
    }
}
