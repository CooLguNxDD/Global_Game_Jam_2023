using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayController : MonoBehaviour
{
    // Start is called before the first frame update
    public CardManager CardSystem;
    public CardPosClass cardPos;

    private Global global;



    //should be false
    // for game testing
    private bool isValidArea = true;

    public void Start()
    {
        CardSystem = this.transform.GetComponentInParent<CardManager>();
        global = CardSystem.global;
    }
    public void MouseDown() {
        //Debug.Log(cardPos.CardIndexOnHand);
    }

    public bool ValidCost()
    {
        if (global.Nutrition > cardPos.card.NutritionCost &&
            global.Water > cardPos.card.WaterCost)
        {
            global.Nutrition -= cardPos.card.NutritionCost; 
            global.Water -= cardPos.card.WaterCost;
            Debug.Log(global.Nutrition);
            Debug.Log(global.Water);
            return true;
        }
        return false;
    }


    public void setValidArea() { isValidArea = true; }


    public void MouseUp()
    {
        if (isValidArea)
        {
            if (ValidCost())
            {
                Debug.Log("played a card");
                CardSystem.PlayCard(cardPos.CardIndexOnHand);
            }
        }
        //Debug.Log("up");
    }
}
