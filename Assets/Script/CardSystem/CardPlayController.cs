using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayController : MonoBehaviour
{
    // Start is called before the first frame update
    public CardManager CardSystem;
    public CardPosClass cardPos;

    //should be false
    // for game testing
    private bool isValidArea = true;

    public void Start()
    {
        CardSystem = this.transform.GetComponentInParent<CardManager>();
    }
    public void MouseDown() {
        Debug.Log(cardPos.CardIndexOnHand);
    }


    public void setValidArea() { isValidArea = true; }


    public void MouseUp()
    {
        if (isValidArea)
        {
            CardSystem.PlayCard(cardPos.CardIndexOnHand);
            Debug.Log("played a card");
        }
        Debug.Log("up");
    }
}
