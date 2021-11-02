using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Node 정보에 대한 처리를 하는 함수를 통합해야 할 듯, Match3의 Node와 합쳐져 있다는 것 자체가 문제
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
