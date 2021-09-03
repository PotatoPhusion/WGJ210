using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector]
    List<Vector2> points;
    [SerializeField, HideInInspector]
    bool isClosed;

    public Path(Vector2 center)
    {
        points = new List<Vector2>
        {
            center + Vector2.left,
            center + Vector2.right
        };

    }

    public Vector2 this[int i]
    {
        get
        {
            return points[i];
        }

    }

    public Vector2[] GetPoints()
    {
        return points.ToArray();
    }

    public int NumPoints
    { 
        get 
        {
            return points.Count;
        }
    }

    public int NumSegments
    {
        get
        {
            return points.Count - (isClosed ? 0 : 1);
        }
    }

    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(anchorPos);
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        return new Vector2[] { points[i], points[LoopIndex(i + 1)] };
    }

    public void MovePoint(int i, Vector2 pos)
    {
        points[i] = pos;
    }

    public void ToggleClosed()
    {
        isClosed = !isClosed;
    }

    public int LoopIndex(int i)
    {
        return (i + points.Count) % points.Count;
    }
}
