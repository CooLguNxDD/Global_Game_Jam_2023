using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile : MonoBehaviour
{
    public Global.TileType type;
    
    public int x;
    public int y;

    public bool isBuildAble = false;

    public void setXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    void Start()
    {
        
    }


}
