using UnityEngine;

public class PathSegment
{

    protected static int autoIncrement = 0;
    protected int id;
    public Vector2Int start;
    public Vector2Int end;
    public string axis;

    public PathSegment(Vector2Int startCoord)
    {
        id = MakeId();
        start = startCoord;
        end = startCoord;
    }

    public PathSegment(Vector2Int startCoord, Vector2Int endCoord)
    {
        id = MakeId();
        start = startCoord;
        end = endCoord;
    }

    public void SetAxis(string axis)
    {
        this.axis = axis;
    }


    protected static int MakeId()
    {
        ++autoIncrement;
        return autoIncrement;
    }

    public int GetId()
    {
        return id;
    }
}
