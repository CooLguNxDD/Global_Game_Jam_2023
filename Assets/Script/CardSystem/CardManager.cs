using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public List<Card> fullDeck = new List<Card>();
    public Queue<Card> currentCards = new Queue<Card>();
    public Queue<Card> discardPile = new Queue<Card>();
    public List<Card> cardOnHand = new List<Card>();

    public Transform[] cardPostion;
    public bool[] avaiableCardSlot;

    public void Shuffle()
    {
        List<Card> tmp = currentCards.ToList<Card>();
        Queue<Card> tmpCurrentCards = new Queue<Card>();
        System.Random r = new System.Random();
        while(tmp.Count > 0)
        {
            int index = r.Next(0, tmp.Count);
            tmpCurrentCards.Enqueue(tmp[index]);
            tmp.RemoveAt(index);
        }
        this.currentCards = tmpCurrentCards;
    }

    public Card DrawCard()
    {

        return currentCards.Dequeue();
    }

    public void PlayCard(int index) {
        discardPile.Enqueue(cardOnHand[index]);
        cardOnHand.RemoveAt(index);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
