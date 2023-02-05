using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Global : MonoBehaviour

{
    public static Global instance;
    public GameObject WaterText;
    public GameObject NutrientText;
    public UnityEvent<string> onNutritionChange;
    public UnityEvent<string> onWaterChange;

    public int currentEnemy = 0;
    public int MaxEnemy = 50;

    public int nutrition;
    public int water;

    public int nutritionProfit;
    public int waterProfit;

    public float AntHPMultiplyer = 1f;

    //global variable
    public int Nutrition
    {
        get
        {
            return nutrition;
        }
        set
        {
            if (nutrition == value)
                return;

            nutrition = value;

            onNutritionChange.Invoke(""+value + $"(+{nutritionProfit})");
        }
    }

    public int Water
    {
        get
        {
            return water;
        }
        set
        {
            if (water == value)
                return;

            water = value;

            onWaterChange.Invoke(""+value + $"(+{waterProfit})");
        }
    }



    public int NutritionProfit
    {
        get
        {
            return nutritionProfit;
        }
        set
        {
            if (nutritionProfit == value)
                return;

            nutritionProfit = value;

            onNutritionChange.Invoke(""+Nutrition + $"(+{value})");
        }
    }
    public int WaterProfit
    {
        get
        {
            return waterProfit;
        }
        set
        {
            if (waterProfit == value)
                return;

            waterProfit = value;

            onWaterChange.Invoke(""+Water + $"(+{value})");
        }
    }


    public void SetNutrition(int value)
    {
        Nutrition = value;
    }

    public void SetWater(int value)
    {
        Water = value;
    }

    public void SetNutritionProfit(int value)
    {
        NutritionProfit = value;
    }

    public void SetWaterProfit(int value)
    {
        WaterProfit = value;
    }

    public void CalculateTileProfit(TileType original, TileType target)
    {
        if (original == TileType.WATER && target == TileType.ROOT)
        {
            WaterProfit++;
        }
        if (original == TileType.NUTRIENT && target == TileType.ROOT)
        {
            NutritionProfit++;
        }
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
            SetNutrition(nutrition+nutritionProfit);
            SetWater(water+waterProfit);
            yield return new WaitForSeconds(3);
        }
        
    }
}
