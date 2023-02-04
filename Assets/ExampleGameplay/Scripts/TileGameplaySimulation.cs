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

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                switch(board[x,y])
                {
                    case (int)TileType.EMPTY:
                        
                        Instantiate(groundPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                            
                        );
                        break;
                    case (int)TileType.ROOT:
                        Instantiate(rootPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        );
                        break;
                    case (int)TileType.WATER:
                        Instantiate(waterPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        );
                        
                        break;
                    case (int)TileType.NUTRIENT:
                        Instantiate(nutrientPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        );
                        
                        break;
                    case (int)TileType.ROCK:
                        Instantiate(rockPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        );
                        
                        break;
                    case (int)TileType.ENEMY_NEST:
                        Instantiate(enemyPrefab, 
                            new Vector3(y * cellSize+initialPosition.y+offsetGrid, -x * cellSize+initialPosition.x+offsetGrid, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            parent.transform
                        );
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
