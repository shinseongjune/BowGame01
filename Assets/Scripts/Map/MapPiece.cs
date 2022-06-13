using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPiece
{
    public int x;
    public int y;
    public int z;

    public Vector3 GetPosition(float tileSize)
    {
        return new Vector3((x * tileSize) + tileSize / 2, (y * tileSize) + tileSize / 2, (z * tileSize) + tileSize / 2);
    }
}
