using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
public class CardPlayController : MonoBehaviour
{
    // Start is called before the first frame update
    public CardManager CardSystem;
    public CardPosClass cardPos;

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
        Debug.Log(previousPosition);
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
        else
        {
            CardBackToEnd();
        }
        return false;
    }

    public void CardBackToEnd()
    {
        this.transform.GetComponent<RectTransform>().DOMove(previousPosition, 1f);
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
