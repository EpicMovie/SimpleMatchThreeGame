using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Node 정보에 대한 처리를 하는 함수를 통합해야 할 듯, Match3의 Node와 합쳐져 있다는 것 자체가 문제
    public EShapeType shapeType;
    public Point index;

    [HideInInspector]
    public Vector2 pos;

    [HideInInspector]
    public NodePiece flipped;

    [HideInInspector]
    public RectTransform rect;

    bool updating;

    const float speedConfig = 16f;

    Image img;

    public void Init(EShapeType InShapeType, Point InPoint, Sprite InPiece)
    {
        flipped = null;

        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();


        shapeType = InShapeType;
        img.sprite = InPiece;

        SetIndex(InPoint);
    }

    public void SetIndex(Point InPoint)
    {
        index = InPoint;

        ResetPos();
        UpdateName();
    }

    public void ResetPos()
    {
        pos = new Vector2(32 + (64 * index.x), -32 - (64 * index.y));
    }

    public void MovePos(Vector2 InDelta)
    {
        rect.anchoredPosition += InDelta * Time.deltaTime * speedConfig;
    }

    public void MovePosTo(Vector2 InDes)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, InDes, Time.deltaTime * speedConfig);
    }

    void UpdateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public bool UpdatePiece()
    {
        if (Vector3.Distance(rect.anchoredPosition, pos) > 1)
        {
            MovePosTo(pos);
            updating = true;

            return true;
        }
        else
        {
            rect.anchoredPosition = pos;
            updating = false;

            return false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (updating)
        {
            return;
        }
        else
        {
            MovePieces.Instance.MovePiece(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MovePieces.Instance.DropPiece();
    }
}
