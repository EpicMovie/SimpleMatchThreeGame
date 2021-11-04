using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EShapeType
{
    Hole = -1, 
    None,
    Cube,
    Sphere,
    Cylinder,
    Pyramid,
    Diamond,
}

public class Match3 : MonoBehaviour
{
    public ArrayLayout boardLayout;
    
    // 이것도 타입 맵으로 정의할 수 있나?
    [Header("UI Elements")]
    public Sprite[] pieces;

    public RectTransform gameBoard;

    [Header("Prefabs")]
    public GameObject nodePiece;

    private const int width = 9;
    private const int height = 14;

    Node[,] board;

    List<NodePiece> update;

    System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void Update()
    {
        foreach (NodePiece nodePiece in update.ToArray())
        {
            if (nodePiece.UpdatePiece() == false)
            {
                update.Remove(nodePiece);
            }
        }
    }

    void StartGame()
    {
        string seed = GetRandomSeed();

        random = new System.Random(seed.GetHashCode());
        update = new List<NodePiece>();

        // Board class를 만든 후 해당 메서드들을 분할하는 것도 좋을 듯?
        InitializeBoard();
        VarifyBoard();
        InstantiateBoard();
    }

    void InitializeBoard()
    {
        board = new Node[width, height];

        for(int y = 0; y < height; y++)
        {
            for(int x = 0;  x < width; x++)
            {
                EShapeType shapeType = (boardLayout.rows[y].row[x]) ?
                    EShapeType.Hole :
                    FillPiece();

                board[x, y] = new Node(
                    shapeType, new Point(x, y)
                );
            }
        }
    }

    void VarifyBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Point p = new Point(x, y);

                EShapeType shapeType = GetTypeAt(p);

                if(shapeType <= EShapeType.None)
                {
                    continue;
                }
                else
                {
                    List<EShapeType> remove = new List<EShapeType>();

                    while (IsConnected(p, true).Count > 0)
                    {
                        shapeType = GetTypeAt(p);

                        if(!remove.Contains(shapeType))
                        {
                            remove.Add(shapeType);
                        }

                        SetTypeAt(p, GetValue(ref remove));
                    }
                }
            }
        }
    }

    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                EShapeType shapeType = board[x, y].shapeType;

                if (shapeType > EShapeType.None)
                {
                    Node node = GetNodeAt(new Point(x, y));

                    GameObject obj = Instantiate(nodePiece, gameBoard);

                    NodePiece piece = obj.GetComponent<NodePiece>();
                    RectTransform rect = obj.GetComponent<RectTransform>();

                    rect.anchoredPosition = GetPosFrom(new Point(x, y));
                    piece.Init(shapeType, new Point(x, y), pieces[(int)shapeType - 1]);
                    node.SetPiece(piece);
                }
            }
        }
    }

    public void ResetPiece(NodePiece InPiece)
    {
        InPiece.ResetPos();
        InPiece.flipped = null;

        update.Add(InPiece);
    }

    public void FlipPieces(Point InA, Point InB)
    {
        if (GetTypeAt(InA) <= EShapeType.None)
        {
            return;
        }

        Node nodeA = GetNodeAt(InA);
        NodePiece pieceA = nodeA.GetNodePiece();

        if (GetTypeAt(InB) > EShapeType.None)
        {
            Node nodeB = GetNodeAt(InB);
            NodePiece pieceB = nodeB.GetNodePiece();

            nodeA.SetPiece(pieceB);
            nodeB.SetPiece(pieceA);

            pieceA.flipped = pieceB;
            pieceB.flipped = pieceA;

            update.Add(pieceA);
            update.Add(pieceB);
        }
        else
        {
            ResetPiece(pieceA);
        }
    }

    List<Point> IsConnected(Point InPoint, bool InMain)
    {
        List<Point> connected = new List<Point>();

        EShapeType shapeType = GetTypeAt(InPoint);
        
        if(shapeType <= EShapeType.None)
        {
            return new List<Point>();
        }

        Point[] dirs =
        {
            Point.Up,
            Point.Right,
            Point.Down,
            Point.Left
        };

        // Check if there are 2 or more same shapes in the directions
        // Without middle matching cases 
        foreach (Point dir in dirs)
        {
            List<Point> line = new List<Point>();

            // Only 3 by 3 Condition checks
            // so just check other two block is equal
            int sameNodes = 0;

            for(int i = 1; i < 3; i++)
            {
                Point check = Point.Add(InPoint, Point.Mult(InPoint, i));

                if(GetTypeAt(check) == shapeType)
                {
                    line.Add(check);

                    sameNodes++;
                }
            }

            // if there are more than one of same shape in the direction,
            // then it is matched 
            if(sameNodes > 1)
            {
                // Add there points to the overarching connected list
                AddPoints(ref connected, line);
            }
        }

        // Check middle matching cases like x000x
        for(int i = 0; i < 2; i++)
        {
            Point[] checks = {
                Point.Add(InPoint, dirs[i]),
                Point.Add(InPoint, dirs[i + 2])
            };

            // Check both sides of the node, if they are the same type, add them to connected
            if(GetTypeAt(checks[0]) == shapeType && GetTypeAt(checks[1]) == shapeType)
            {
                AddPoints(ref connected, new List<Point>(checks));
            }
        }
        
        // Check 2 x 2 matching cases 
        for(int i = 0; i < dirs.Length; i++)
        {
            Point[] checks = {
                Point.Add(InPoint, dirs[i % dirs.Length]),
                Point.Add(InPoint, dirs[(i + 1) % dirs.Length]),
                Point.Add(InPoint, dirs[(i + 2) % dirs.Length]),
                Point.Add(InPoint, dirs[(i + 3) % dirs.Length])
            };

            bool isSameType = true;

            foreach(Point check in checks)
            {
                if(GetTypeAt(check) != shapeType)
                {
                    isSameType &= false;
                    break;
                }
            }

            if(isSameType)
            {
                AddPoints(ref connected, new List<Point>(checks));
            }
        }

        if(InMain)
        {
            for (int i = 0; i < connected.Count; i++)
            {
                AddPoints(ref connected, IsConnected(connected[i], false));
            }
        }

        if(connected.Count > 0)
        {
            connected.Add(InPoint);
        }

        return new List<Point>(connected);
    }

    void AddPoints(ref List<Point> InConnected, List<Point> InAdd)
    {
        foreach(Point point in InAdd)
        {
            if(InConnected.Contains(point) == false)
            {
                InConnected.Add(point);
            }
        }
    }

    EShapeType FillPiece()
    {
        return (EShapeType) (random.Next(0, 100) * (pieces.Length / 100) + 1);
    }

    EShapeType GetTypeAt(Point InPoint)
    {
        if(
            InPoint.x < 0 || InPoint.x >= width || 
            InPoint.y < 0 || InPoint.y >= height
            )
        {
            return EShapeType.None;
        }

        return board[InPoint.x, InPoint.y].shapeType;
    }

    void SetTypeAt(Point InPoint, EShapeType InShapeType)
    {
        board[InPoint.x, InPoint.y].shapeType = InShapeType;
    }

    EShapeType GetValue(ref List<EShapeType> InRemove)
    {
        List<EShapeType> available = new List<EShapeType>();

        for(int i = 0; i < pieces.Length; i++)
        {
            available.Add((EShapeType)(i + 1));
        }

        foreach(EShapeType shapeType in InRemove)
        {
            available.Remove(shapeType);
        }

        if(available.Count <= 0 )
        {
            return EShapeType.None;
        }

        return available[random.Next(0, available.Count)];
    }

    Node GetNodeAt(Point InPoint)
    {
        return board[InPoint.x, InPoint.y];
    }

    string GetRandomSeed()
    {
        string seed = "";
        string acceptableChars = "wjlfawejlkdfnmkbierofnlqwejfq;j;lwd;129031804gndl";

        for(int i = 0; i < 20; i++)
        {
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        }

        return seed;
    }

    public Vector2 GetPosFrom(Point InPoint)
    {
        return new Vector2(32 + (64 * InPoint.x), -32 - (64 * InPoint.y));
    }
}

[System.Serializable]
public class Node
{
    public EShapeType shapeType;
    public Point index;
    public NodePiece piece;

    public Node(EShapeType InShapeType, Point InIndex)
    {
        shapeType = InShapeType;
        index = InIndex;
    }

    public void SetPiece(NodePiece InNodePiece)
    {
        piece = InNodePiece;
        shapeType = (piece == null) ? EShapeType.Hole : piece.shapeType;

        if (piece == null)
        {
            return;
        }
        else
        {
            piece.SetIndex(index);
        }
    }

    public NodePiece GetNodePiece()
    {
        return piece;
    }
}


