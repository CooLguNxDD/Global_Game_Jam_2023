using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "card", menuName = "Cards")]
public class Card : ScriptableObject
{
    // Card idenitifier
    public int CardID;
    public Global.TileType type;

    // Card Status
    public int NutritionCost;
    public int WaterCost;
    public GameObject spwanableObject;
    public Tower scriptableObject;
    
    public string Name;
    [TextArea]
    public string Description;

    public Sprite CardImage;
    
    

    public void Awake()
    {
        // Debug.Log("this is correct!!!");
        
        if (type == Global.TileType.TOWER)
        {
            spwanableObject.GetComponent<TowerSampleScript>().tower = scriptableObject;
        }

    }
}
