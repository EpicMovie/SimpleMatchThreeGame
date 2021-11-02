using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Node ������ ���� ó���� �ϴ� �Լ��� �����ؾ� �� ��, Match3�� Node�� ������ �ִٴ� �� ��ü�� ����
    public EShapeType ShapeType;
    public Point Index;

    [HideInInspector]
    public Vector2 Pos;

    [HideInInspector]
    public RectTransform Rect;

    bool Updating;

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

    public bool UpdatePiece()
    {
        return true; 
        // return false if it is not moving ... 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(Updating)
        {
            return;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
