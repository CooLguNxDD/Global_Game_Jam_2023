using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
public class CardPlayController : MonoBehaviour
{
    // Start is called before the first frame update
    public CardManager cardSystem;
    public CardPosClass CardPosClass;

    public Global global;
    private Vector3 _previousPosition;

    public void Start()
    {
        cardSystem = this.transform.GetComponentInParent<CardManager>();
        global = cardSystem.global;

    }
    public void MouseDown() {
        //start dragging
        
        global.draggingCard = CardPosClass.card;

        if (_previousPosition == Vector3.zero)
        {
            _previousPosition = this.transform.GetComponent<RectTransform>().position;
        }

        CardPosClass.CardPos.GetComponent<CardDisplaySetting>().SetAlpha(0.75f);
        this.transform.GetComponent<RectTransform>().localScale = Vector3.one * 0.5f;

        Debug.Log(_previousPosition);
        //Debug.Log(cardPos.CardIndexOnHand);
    }

    public bool ValidCost()
    {
        if (global.Nutrition > CardPosClass.card.NutritionCost &&
            global.Water > CardPosClass.card.WaterCost)
        {
            global.Nutrition -= CardPosClass.card.NutritionCost; 
            global.Water -= CardPosClass.card.WaterCost;
            Debug.Log(global.Nutrition);
            Debug.Log(global.Water);
            return true;
        }
        
        return false;
    }

    public void CardBackToEnd()
    {
        
        this.transform.GetComponent<RectTransform>().DOMove(_previousPosition, 0.25f);
    }

    public void MouseUp()
    {
        //start dragging
        
        if (global.isValidLocation && global.draggingCard)
        {
            if (ValidCost())
            {
                Debug.Log("played a card");
                cardSystem.PlayCard(CardPosClass.CardIndexOnHand);
                return;
            }
            Debug.Log("not enough cost");
        }
        Debug.Log("invalid area");
        transform.GetComponent<RectTransform>().localScale = Vector3.one;
        CardPosClass.CardPos.GetComponent<CardDisplaySetting>().SetAlpha(1f);
        
        CardBackToEnd();
        //set dragging to false
        global.draggingCard = null;
        //Debug.Log("up");
    }
}
