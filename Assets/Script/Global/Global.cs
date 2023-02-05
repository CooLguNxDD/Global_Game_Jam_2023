using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global

{
    //global variable
    public static int Nutrition = 100;
    public static int Water = 100;
    
    // game controller elements
    public static bool isValidLocation;
    public static GameObject buildOn;
    public static Card draggingCard;
    
    public enum TileType
    {
        EMPTY,
        ROOT,
        WATER,
        NUTRIENT,
        ROCK,
        ENEMY_NEST,
        TOWER,
    }
}
