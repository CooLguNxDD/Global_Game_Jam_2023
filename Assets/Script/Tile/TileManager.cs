using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoBehaviour
{
    public enum TileType
    {
        EMPTY,
        ROOT,
        WATER,
        NUTRIENT,
        ROCK,
        ENEMY_NEST
    }

    public Global global;

    public static int row = 50;
    public static int column = 50;

    public float emptyRatio = 0.7f;

    public float waterRatio = 0.1f;

    public float nutrientRatio = 0.1f;
    public float rockRatio = 0.1f;

    public float offsetGrid = 0.1f;

    public int[,] board = new int[row, column];

    // for generating terrain
    float offsetX;
    float offsetY;

    public void changeGrid(int x, int y, int tile, ref int[,] board) {
        board[x,y] = tile;
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
                    changeGrid(x,y, (int)TileType.EMPTY, ref board);
                    continue;
                }

                float height_value = getHeightByXY(x,y);
                if (height_value < emptyRatio)
                {
                    changeGrid(x,y, (int)TileType.EMPTY, ref board);
                }
                else if (height_value < emptyRatio + waterRatio)
                {
                    changeGrid(x,y, (int)TileType.WATER, ref board);
                }
                else if (height_value < emptyRatio + waterRatio + nutrientRatio)
                {
                    changeGrid(x,y, (int)TileType.NUTRIENT, ref board);
                }
                else
                {
                    changeGrid(x,y, (int)TileType.ROCK, ref board);
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
                    // Debug.Log("Color"+(int)TileType.EMPTY);
                    switch(board[x,y])
                    {
                        case (int)TileType.EMPTY:
                            Gizmos.color = Color.yellow;
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
                    }
                    // Gizmos.color = Color.yellow;
                    Vector3 cellPos = new Vector3(x * cellSize+initialPosition.x+offsetGrid, y * cellSize+initialPosition.y+offsetGrid, 0);
                    Gizmos.DrawWireCube(cellPos, new Vector3((1-offsetGrid*2), (1-offsetGrid*2), 0f) * cellSize);
                }
                
                
            }
        }
    }
}
