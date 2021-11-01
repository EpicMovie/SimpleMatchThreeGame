using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point 
{
    public int X;
    public int Y;

    public Point(int InX, int InY)
    {
        X = InX;
        Y = InY;
    }

    public void Add(Point InPoint)
    {
        X += InPoint.X;
        Y += InPoint.Y;
    }

    public void Mul(int InScale)
    {
        X *= InScale;
        Y *= InScale;
    }

    public Vector2 ToVector()
    {
        return new Vector2(X, Y);
    }

    public bool Equals(Point InPoint)
    {
        return X == InPoint.X && Y == InPoint.Y;
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
        return new Point(InPoint.X + InOther.X, InPoint.Y + InOther.Y);
    }

    public static Point Mul(Point InPoint, int InScale)
    {
        return new Point(InPoint.X * InScale, InPoint.Y * InScale);
    }

    public static Point Clone(Point InPoint)
    {
        return new Point(InPoint.X, InPoint.Y);
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

