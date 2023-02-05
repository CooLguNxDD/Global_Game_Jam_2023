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

    public GameObject target_art= null;

    public Sprite branchAll;
    public Sprite branchH;
    public Sprite branchV;
    public Sprite branchLNE;
    public Sprite branchLNW;
    public Sprite branchLSE;
    public Sprite branchLSW;
    public Sprite branchTDown;
    public Sprite branchTUp;
    public Sprite branchTLeft;
    public Sprite branchTRight;


    public void setXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    void Start()
    {
        
    }

    public void UpdateArt()
    {
        if (target_art == null) return;
        bool RootUp = false;
        bool RootLeft = false;
        bool RootDown = false;
        bool RootRight = false;
        if (y-1 >=0) RootDown = TileManager.instance.board[x,y-1] == (int)Global.TileType.ROOT;
        if (x-1 >=0) RootLeft = TileManager.instance.board[x-1,y] == (int)Global.TileType.ROOT;
        if (x+1 < TileManager.column) RootRight = TileManager.instance.board[x+1,y] == (int)Global.TileType.ROOT;
        if (y+1 < TileManager.row) RootUp = TileManager.instance.board[x,y+1] == (int)Global.TileType.ROOT;

        if (RootUp && RootDown && RootLeft && RootRight)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchAll;
        }
        else if (!RootUp && RootDown && RootLeft && RootRight)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchTDown;
        }
        else if (RootUp && !RootDown && RootLeft && RootRight)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchTUp;
        }
        else if (RootUp && RootDown && !RootLeft && RootRight)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchTRight;
        }
        else if (RootUp && RootDown && RootLeft && !RootRight)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchTLeft;
        }
        else if (RootUp && !RootDown && !RootLeft && RootRight)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchLNE;
        }
        else if (RootUp && !RootDown && RootLeft && !RootRight)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchLNW;
        }
        else if (!RootUp && RootDown && RootLeft && !RootRight)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchLSW;
        }
        else if (!RootUp && RootDown && !RootLeft && RootRight)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchLSE;
        }
        else if (RootLeft || RootRight)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchH;
        }
        else if (RootUp || RootDown)
        {
            target_art.transform.GetComponent<SpriteRenderer>().sprite = branchV;
        }
    }

}
