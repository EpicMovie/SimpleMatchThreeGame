using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EShapeType : int
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
    public Sprite[] Pieces;

    private const int width = 9;
    private const int height = 14;

    Node[,] board;

    System.Random random;

    // Start is called before the first frame update
    void Start()
    {
           
    }

    void StartGame()
    {
        string seed = GetRandomSeed();

        random = new System.Random(seed.GetHashCode());

        InitializeBoard();
    }

    void InitializeBoard()
    {
        board = new Node[width, height];

        for(int y = 0; y < height; y++)
        {
            for(int x = 0;  x < width; x++)
            {
                board[x, y] = new Node(
                    (int)EShapeType.Hole, new Point(x, y)
                );
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

[System.Serializable]
public class Node
{
    public EShapeType ShapeType;
    public Point Index;

    public Node(EShapeType InShapeType, Point InIndex)
    {
        ShapeType = InShapeType;
        Index = InIndex;
    }
}
