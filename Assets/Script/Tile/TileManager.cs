using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoBehaviour
{
    enum TileType
    {
        EMPTY,
        ROOT,
        RESOURCE,
        ENEMY_NEST
    }

    public TileType[,] board = new TileType[50, 50];


    public void changeGrid(int x, int y, TileType tile) {
        board[x,y] = tile;
    }
}
