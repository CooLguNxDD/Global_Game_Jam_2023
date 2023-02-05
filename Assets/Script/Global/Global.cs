using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Global : MonoBehaviour

{
    public static Global instance;
    public GameObject WaterText;
    public GameObject NutrientText;

    //global variable
    public int Nutrition = 100;
    
    public int Water = 100;

    public int NutritionProfit = 1;
    public int WaterProfit = 1;

    public void SetNutrition(int value)
    {
        Nutrition = value;
        NutrientText.transform.GetComponent<TextMeshProUGUI>().text = ""+value+" (+"+NutritionProfit+")";
    }

    public void SetWater(int value)
    {
        Water = value;
        WaterText.transform.GetComponent<TextMeshProUGUI>().text = ""+value+" (+"+WaterProfit+")";
    }
    
    // game controller elements
    public bool isValidLocation;
    public GameObject buildOn;
    public Card draggingCard;

    
    
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

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        NutrientText.transform.GetComponent<TextMeshProUGUI>().text = ""+Nutrition+" (+"+NutritionProfit+")";
        WaterText.transform.GetComponent<TextMeshProUGUI>().text = ""+Water+" (+"+WaterProfit+")";
        StartCoroutine(CalculateProfit());
    }

    IEnumerator CalculateProfit()
    {
        while(true)
        {
            SetNutrition(Nutrition+NutritionProfit);
            SetWater(Water+WaterProfit);
            yield return new WaitForSeconds(3);
        }
        
    }
}
