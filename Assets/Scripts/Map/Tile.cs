using UnityEngine;

public class Tile
{
    public int x;
    public int y;
    public int z;

    public float size;

    public Vector3 position
    {
        get
        {
            return new Vector3((x * size) + (size / 2), y * size / 2, (z * size) + (size / 2));
        }
    }

    public TileLine topLine;
    public TileLine bottomLine;
    public TileLine leftLine;
    public TileLine rightLine;

    public Tile(float size, int x, int y, int z, TileLine topLine, TileLine bottomLine, TileLine leftLine, TileLine rightLine)
    {
        this.size = size;
        this.x = x;
        this.y = y;
        this.z = z;

        this.topLine = topLine;
        this.bottomLine = bottomLine;
        this.leftLine = leftLine;
        this.rightLine = rightLine;
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