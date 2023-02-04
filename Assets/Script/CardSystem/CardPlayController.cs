using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
public class CardPlayController : MonoBehaviour
{
    // Start is called before the first frame update
    public CardManager CardSystem;
    public CardPosClass cardPosClass;

    private Global global;
    private Vector3 previousPosition;



    //should be false
    // for game testing
    private bool isValidArea = true;

    public void Start()
    {
        CardSystem = this.transform.GetComponentInParent<CardManager>();
        global = CardSystem.global;

    }
    public void MouseDown() {
        if (previousPosition == Vector3.zero)
        {
            previousPosition = this.transform.GetComponent<RectTransform>().position;
        }

        cardPosClass.CardPos.GetComponent<CardDisplaySetting>().SetAlpha(0.75f);
        this.transform.GetComponent<RectTransform>().localScale = Vector3.one * 0.5f;

        Debug.Log(previousPosition);
        //Debug.Log(cardPos.CardIndexOnHand);
    }

    public bool ValidCost()
    {
        if (global.Nutrition > cardPosClass.card.NutritionCost &&
            global.Water > cardPosClass.card.WaterCost)
        {
            global.Nutrition -= cardPosClass.card.NutritionCost; 
            global.Water -= cardPosClass.card.WaterCost;
            Debug.Log(global.Nutrition);
            Debug.Log(global.Water);
            return true;
        }
        else
        {
            CardBackToEnd();
        }
        return false;
    }

    public void CardBackToEnd()
    {
        
        this.transform.GetComponent<RectTransform>().DOMove(previousPosition, 0.25f);
    }


    public void setValidArea() { isValidArea = true; }


    public void MouseUp()
    {
        if (isValidArea)
        {
            if (ValidCost())
            {
                Debug.Log("played a card");
                CardSystem.PlayCard(cardPosClass.CardIndexOnHand);
            }
        }
        this.transform.GetComponent<RectTransform>().localScale = Vector3.one;
        cardPosClass.CardPos.GetComponent<CardDisplaySetting>().SetAlpha(1f);
        //Debug.Log("up");
    }
}
