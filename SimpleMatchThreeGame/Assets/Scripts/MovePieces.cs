using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
    public static MovePieces Instance;

    Match3 Game;

    NodePiece Moving;
    Point NewIndex;

    Vector2 MouseStartPos;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Game = GetComponent<Match3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving != null)
        {
            Vector2 dir = ((Vector2)Input.mousePosition - MouseStartPos);
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

            NewIndex = Point.Clone(Moving.index);
            Point add = Point.Zero;

            // if our mouse is 32 pixels away from the starting point of the mouse,
            if (dir.magnitude > 32)
            {
                // Make add either (1, 0) || (-1, 0) || (0, 1) || (0, -1)
                // depending on the direction of the mouse point 
                // to make simple direction to move Node
                if (aDir.x > aDir.y)
                {
                    add = new Point((nDir.x > 0) ? 1 : -1, 0);
                }
                else if(aDir.y > aDir.x)
                {
                    add = new Point(0, (nDir.y > 0) ? -1 : 1);
                }
            }

            NewIndex.Add(add);

            Vector2 pos = Game.GetPosFrom(Moving.index);

            if (NewIndex.Equals(Moving.index) == false)
            {
                pos += Point.Mult(new Point(add.x, -add.y), 16).ToVector();
            }

            Moving.MovePosTo(pos);
        }
    }

    public void MovePiece(NodePiece InNodePiece)
    {
        if (Moving != null)
        {
            return;
        }
        else
        {
            Moving = InNodePiece;

            MouseStartPos = Input.mousePosition;
        }
    }

    public void DropPiece()
    {
        if (Moving == null)
        {
            return;
        }
        else
        {
            if (NewIndex.Equals(Moving.index) == false)
            {
                Game.FlipPieces(Moving.index, NewIndex, true);
            }
            else
            {
                Game.ResetPiece(Moving);
            }

            Moving = null;
        }
    }
}
