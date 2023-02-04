using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGameplaySimulation : MonoBehaviour
{

    public GameObject parent;
    public enum TileType
    {
        EMPTY,
        ROOT,
        WATER,
        NUTRIENT,
        ROCK,
        ENEMY_NEST
    }

    public int[,] board = {
            {0, 1, 0, 0},
            {0, 1, 1, 0},
            {2, 0, 1, 0},
            {4, 4, 0, 0},
            {0, 0, 3, 0},
            {5, 0, 0, 0},
    };

    public Transform[,] board_pieces;

    public static int column        = 4;
    public static int row           = 6;
    public float cellSize           = 1;
    public float offsetGrid         = 0.1f;
    public Vector3 initialPosition = new Vector3(0f,0f,0f);

    public GameObject enemyPrefab;
    public GameObject groundPrefab;
    public GameObject rootPrefab;
    public GameObject waterPrefab;
    public GameObject nutrientPrefab;
    public GameObject rockPrefab;

    bool checkIfBuildableAt(int x, int y)
    {
        if (y+1 < column)
            if (board[x,y+1] == (int)TileType.ROOT) return true;
        if (y-1 >= 0)
            if (board[x,y-1] == (int)TileType.ROOT) return true;
        if (x+1 < row)
            if (board[x+1,y] == (int)TileType.ROOT) return true;
        if (x-1 >= 0)
            if (board[x-1,y] == (int)TileType.ROOT) return true;
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        board_pieces = new Transform[row,column];
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                switch(board[x,y])
                {
                    case (int)TileType.EMPTY:
                        
                        board_pieces[x,y] = Instantiate(groundPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                            
                        ).transform;
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = checkIfBuildableAt(x, y);
                        break;
                    case (int)TileType.ROOT:
                        board_pieces[x,y] = Instantiate(rootPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        ).transform;
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        break;
                    case (int)TileType.WATER:
                        board_pieces[x,y] = Instantiate(waterPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        ).transform;
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = checkIfBuildableAt(x, y);
                        break;
                    case (int)TileType.NUTRIENT:
                        board_pieces[x,y] = Instantiate(nutrientPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        ).transform;
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = checkIfBuildableAt(x, y);
                        break;
                    case (int)TileType.ROCK:
                        board_pieces[x,y] = Instantiate(rockPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        ).transform;
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        break;
                    case (int)TileType.ENEMY_NEST:
                        board_pieces[x,y] = Instantiate(enemyPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        ).transform;
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                switch(board[x,y])
                {
                    case (int)TileType.EMPTY:
                        Gizmos.color = Color.yellow;
                        break;
                    case (int)TileType.ROOT:
                        Gizmos.color = new Color(0.4f, 0.25f, 0.13f, 1f);
                        break;
                    case (int)TileType.WATER:
                        Gizmos.color = Color.blue;
                        break;
                    case (int)TileType.NUTRIENT:
                        Gizmos.color = Color.magenta;
                        break;
                    case (int)TileType.ROCK:
                        Gizmos.color = Color.grey;
                        break;
                    case (int)TileType.ENEMY_NEST:
                        Gizmos.color = Color.red;
                        break;
                }
                // Gizmos.color = Color.yellow;
                Vector3 cellPos = new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0);
                Gizmos.DrawWireCube(cellPos, new Vector3((1-offsetGrid*2), (1-offsetGrid*2), 0f) * cellSize);
            }
        }
    }
}
