using UnityEngine;

public struct Tile
{
    public int x;
    public int y;

    public Vector3 position
    {
        get
        {
            return new Vector3(x, 0, y);
        }
    }

    public TileLine topLine;
    public TileLine bottomLine;
    public TileLine leftLine;
    public TileLine rightLine;

    public Tile(int x, int y, TileLine topLine, TileLine bottomLine, TileLine leftLine, TileLine rightLine)
    {
        this.x = x;
        this.y = y;

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