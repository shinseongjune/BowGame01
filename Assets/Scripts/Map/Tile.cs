using UnityEngine;

public class Tile
{
    public int x;
    public float y;
    public int z;


    public Tile(int x, float y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public class TileLine
{
    public Vector3 position;

    public bool isHorizontal;

    public TileLine(Vector3 position, bool isHorizontal)
    {
        this.position = position;
        this.isHorizontal = isHorizontal;
    }
}