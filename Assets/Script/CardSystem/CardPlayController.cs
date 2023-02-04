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
    
    private CardDisplaySetting _cardDisplaySetting;
    private RectTransform _rectTransform;

    public void Start()
    {
        cardSystem = this.transform.GetComponentInParent<CardManager>();
        _cardDisplaySetting = CardPosClass.CardPos.GetComponent<CardDisplaySetting>();
        _rectTransform = transform.GetComponent<RectTransform>();
        global = cardSystem.global;

    }
    public void MouseDown() {
        //start dragging
        
        global.draggingCard = CardPosClass.card;

        if (_previousPosition == Vector3.zero)
        {
            _previousPosition = _rectTransform.position;
        }

        _cardDisplaySetting.SetAlpha(0.75f);
        _cardDisplaySetting.SetOnDragging(true);
        _rectTransform.localScale = Vector3.one * 0.5f;

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
        
        _rectTransform.DOMove(_previousPosition, 0.25f);
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
        _rectTransform.localScale = Vector3.one;
        _cardDisplaySetting.SetAlpha(1f);
        _cardDisplaySetting.SetOnDragging(false);
        
        CardBackToEnd();
        //set dragging to false
        global.draggingCard = null;
        //Debug.Log("up");
    }
}
