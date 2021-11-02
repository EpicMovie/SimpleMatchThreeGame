using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodePiece : MonoBehaviour
{
    // Node ������ ���� ó���� �ϴ� �Լ��� �����ؾ� �� ��, Match3�� Node�� ������ �ִٴ� �� ��ü�� ����
    public EShapeType ShapeType;
    public Point Index;

    [HideInInspector]
    public Vector2 Pos;

    [HideInInspector]
    public RectTransform Rect;

    Image Img;

    public void Init(EShapeType InShapeType, Point InPoint, Sprite InPiece)
    {
        Img = GetComponent<Image>();
        Rect = GetComponent<RectTransform>();

        ShapeType = InShapeType;
        Img.sprite = InPiece;

        SetIndex(InPoint);
    }

    public void SetIndex(Point InPoint)
    {
        Index = InPoint;

        ResetPos();
        UpdateName();
    }

    public void ResetPos()
    {
        Pos = new Vector2(32 + (64 * Index.X), -32 - (64 * Index.Y));
    }

    void UpdateName()
    {
        transform.name = "Node [" + Index.X + ", " + Index.Y + "]";
    }
}
