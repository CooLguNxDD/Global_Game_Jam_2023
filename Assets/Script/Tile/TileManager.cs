using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoBehaviour
{

    public static TileManager instance = null;

    public static int row = 50;
    public static int column = 50;

    public float emptyRatio = 0.7f;

    public float waterRatio = 0.1f;

    public float nutrientRatio = 0.1f;
    public float rockRatio = 0.1f;

    public float offsetGrid = 0.1f;

    public int[,] board = new int[row, column];
    public Transform[,] board_pieces = new Transform[row, column];

    // for generating terrain
    float offsetX;
    float offsetY;

    public GameObject parent;
    public GameObject enemyPrefab;
    public GameObject groundPrefab;
    public GameObject rootPrefab;
    public GameObject waterPrefab;
    public GameObject nutrientPrefab;
    public GameObject rockPrefab;

    public void changeGrid(int x, int y, int tile, ref int[,] board) {
        board[x,y] = tile;
    }

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    bool checkIfBuildableAt(int x, int y)
    {
        if (y+1 < column)
            if (board[x,y+1] == (int)Global.TileType.ROOT) return true;
        if (y-1 >= 0)
            if (board[x,y-1] == (int)Global.TileType.ROOT) return true;
        if (x+1 < row)
            if (board[x+1,y] == (int)Global.TileType.ROOT) return true;
        if (x-1 >= 0)
            if (board[x-1,y] == (int)Global.TileType.ROOT) return true;
        return false;
    }

    // make the neightbor tile to be buildable if the type is EMPTY, WATER or NUTRIENT
    public void updateNeighborBuildableAt(int x, int y)
    {
        if (board[x,y] != (int)Global.TileType.ROOT) return;
        if (y+1 < column)
            board_pieces[x,y+1].Find("Square").GetComponent<Tile>().isBuildAble = checkifTileTypeBuildable(x,y+1);
        if (y-1 >= 0)
            board_pieces[x,y-1].Find("Square").GetComponent<Tile>().isBuildAble = checkifTileTypeBuildable(x,y-1);
        if (x+1 < row)
            board_pieces[x+1,y].Find("Square").GetComponent<Tile>().isBuildAble = checkifTileTypeBuildable(x+1,y);
        if (x-1 >= 0)
            board_pieces[x-1,y].Find("Square").GetComponent<Tile>().isBuildAble = checkifTileTypeBuildable(x-1,y);
    }

    bool checkifTileTypeBuildable(int x, int y)
    {
        if (board[x,y] == (int)Global.TileType.ENEMY_NEST) return false;
        if (board[x,y] == (int)Global.TileType.ROCK) return false;
        if (board[x,y] == (int)Global.TileType.ROOT) return false;
        return true;
    }

    public Vector3 initialPosition = new Vector3(0f,0f,0f);
    public float cellSize = 1;

    void Start()
    {
        offsetX = UnityEngine.Random.Range(0f, 1f)*999;
        offsetY = UnityEngine.Random.Range(0f, 1f)*999;
        Debug.Log("hi");
        for (int x=0;x<column;x++)
        {
            for (int y=0;y<row;y++)
            {
                if (x == Math.Floor((column+1)/2f) && y == row-1)
                {
                    changeGrid(x,y, (int)Global.TileType.ROOT, ref board);
                    continue;
                }

                float height_value = getHeightByXY(x,y);
                if (height_value < emptyRatio)
                {
                    changeGrid(x,y, (int)Global.TileType.EMPTY, ref board);
                }
                else if (height_value < emptyRatio + waterRatio)
                {
                    changeGrid(x,y, (int)Global.TileType.WATER, ref board);
                }
                else if (height_value < emptyRatio + waterRatio + nutrientRatio)
                {
                    changeGrid(x,y, (int)Global.TileType.NUTRIENT, ref board);
                }
                else
                {
                    changeGrid(x,y, (int)Global.TileType.ROCK, ref board);
                }
            }
        }

        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < row; y++)
            {
                switch(board[x,y])
                {
                    case (int)Global.TileType.EMPTY:
                        
                        board_pieces[x,y] = Instantiate(groundPrefab, 
                            new Vector3(x * cellSize+initialPosition.x+offsetGrid, y * cellSize+initialPosition.y+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                            
                        ).transform;
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = checkIfBuildableAt(x, y);
                        break;
                    case (int)Global.TileType.ROOT:
                        board_pieces[x,y] = Instantiate(rootPrefab, 
                            new Vector3(x * cellSize+initialPosition.x+offsetGrid, y * cellSize+initialPosition.y+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        ).transform;
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        break;
                    case (int)Global.TileType.WATER:
                        board_pieces[x,y] = Instantiate(waterPrefab, 
                            new Vector3(x * cellSize+initialPosition.x+offsetGrid, y * cellSize+initialPosition.y+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        ).transform;
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = checkIfBuildableAt(x, y);
                        break;
                    case (int)Global.TileType.NUTRIENT:
                        board_pieces[x,y] = Instantiate(nutrientPrefab, 
                            new Vector3(x * cellSize+initialPosition.x+offsetGrid, y * cellSize+initialPosition.y+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        ).transform;
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = checkIfBuildableAt(x, y);
                        break;
                    case (int)Global.TileType.ROCK:
                        board_pieces[x,y] = Instantiate(rockPrefab, 
                            new Vector3(x * cellSize+initialPosition.x+offsetGrid, y * cellSize+initialPosition.y+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        ).transform;
                        board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        break;
                    case (int)Global.TileType.ENEMY_NEST:
                        board_pieces[x,y] = Instantiate(enemyPrefab, 
                            new Vector3(x * cellSize+initialPosition.x+offsetGrid, y * cellSize+initialPosition.y+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        ).transform;
                        break;
                }
            }
        }
    }

    float getHeightByXY(int x, int y) {
        return Mathf.PerlinNoise((x+offsetX)/2f, (y+offsetY)/2f);
    }

    private void OnDrawGizmos()
    {
        
        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < row; y++)
            {
                if (x == Math.Floor((column+1)/2f) && y == row-1)
                {
                    Gizmos.color = Color.green;
                    Vector3 cellPos = new Vector3(x * cellSize+initialPosition.x, y * cellSize+initialPosition.y, 1);
                    Gizmos.DrawIcon(cellPos, "green_box.png", true);
                } else
                {
                    // Debug.Log(board[x,y]);
                    // Debug.Log("Color"+(int)Global.TileType.EMPTY);
                    
                    switch(board[x,y])
                    {
                        case (int)Global.TileType.EMPTY:
                            Gizmos.color = Color.yellow;
                            break;
                        case (int)Global.TileType.WATER:
                            Gizmos.color = Color.blue;
                            break;
                        case (int)Global.TileType.NUTRIENT:
                            Gizmos.color = Color.magenta;
                            break;
                        case (int)Global.TileType.ROCK:
                            Gizmos.color = Color.grey;
                            break;
                    }
                    // Gizmos.color = Color.yellow;
                    Vector3 cellPos = new Vector3(x * cellSize+initialPosition.x+offsetGrid, y * cellSize+initialPosition.y+offsetGrid, 0);
                    Gizmos.DrawWireCube(cellPos, new Vector3((1-offsetGrid*2), (1-offsetGrid*2), 0f) * cellSize);
                }
                
                
            }
        }
    }
}
