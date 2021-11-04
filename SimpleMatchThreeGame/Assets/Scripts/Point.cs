using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point 
{
    public int x;
    public int y;

    public Point(int InX, int InY)
    {
        x = InX;
        y = InY;
    }

    public void Add(Point InPoint)
    {
        x += InPoint.x;
        y += InPoint.y;
    }

    public void Mult(int InScale)
    {
        x *= InScale;
        y *= InScale;
    }

    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }

    public override bool Equals(object obj)
    {
        Point point = (Point)obj;

        return Equals(point);
    }

    public override int GetHashCode()
    {
        int ix = (int)((x + 2f) / 0.2f);
        int iy = (int)((y + 2f) / 0.2f);

        return (int)((ix * 73856093) ^ (iy * 19349663)) % 200;
    }

    public bool Equals(Point InPoint)
    {
        return x == InPoint.x && y == InPoint.y;
    }

    public static Point FromVector(Vector2 InVector)
    {
        return new Point((int)InVector.x, (int)InVector.y);
    }

    public static Point FromVector(Vector3 InVector)
    {
        return new Point((int)InVector.x, (int)InVector.y);
    }

    public static Point Add(Point InPoint, Point InOther)
    {
        return new Point(InPoint.x + InOther.x, InPoint.y + InOther.y);
    }

    public static Point Mult(Point InPoint, int InScale)
    {
        return new Point(InPoint.x * InScale, InPoint.y * InScale);
    }

    public static Point Clone(Point InPoint)
    {
        return new Point(InPoint.x, InPoint.y);
    }

    public static Point Zero
    {
        get { return new Point(0, 0); }
    }

    public static Point One
    {
        get { return new Point(1, 1); }
    }

    public static Point Up
    {
        get { return new Point(0, 1); }
    }

    public static Point Down
    {
        get { return new Point(0, -1); }
    }

    public static Point Left
    {
        get { return new Point(-1, 0); }
    }

    public static Point Right
    {
        get { return new Point(1, 0); }
    }
}

