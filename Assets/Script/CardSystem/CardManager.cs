using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // full deck
    public List<Card> fullDeck = new List<Card>();

    // current cards can be drawed
    public Queue<Card> currentCards = new Queue<Card>();

    //discard pile for special deck build
    public Queue<Card> discardPile = new Queue<Card>();

    // the cards that in player's hand
    public List<CardPosClass> cardOnHand = new List<CardPosClass>();

    public int MaxCardOnHand = 5;
    public GameObject CardDisplayPrefab;

    // index to check each unique card draw to player' hands
    //start from 0 to infinite
    private int CardIndexOnHand; 
    private int CardCount;

    public void Awake()
    {
        CardIndexOnHand = 0;
        CardCount = 0;
    }
    public void Shuffle()
    {
        List<Card> tmp = fullDeck.ToList<Card>();
        Queue<Card> tmpCurrentCards = new Queue<Card>();
        System.Random r = new System.Random();
        while(tmp.Count > 0)
        {
            int index = r.Next(0, tmp.Count);
            tmpCurrentCards.Enqueue(tmp[index]);
            tmp.RemoveAt(index);
        }
        this.currentCards = tmpCurrentCards;

        Debug.Log(currentCards.Count);
    }

    public void DrawCard()
    {
        if (currentCards.Count == 0)
        {
            Shuffle();
        }
        if (CardCount < MaxCardOnHand)
        {
            // dequeue when draw a card
            Card drawCard = currentCards.Dequeue();
            Debug.Log(drawCard.Name);
            DisplayCardOnHand(drawCard);
            CardCount++;
        }
    }

    public void DisplayCardOnHand(Card drawCard)
    {
        // spawn a new position to display card!

        GameObject newCardPos = Instantiate(CardDisplayPrefab);
        CardPosClass cardPosClass = new CardPosClass();

        newCardPos.GetComponent<CardDisplaySetting>().card = drawCard;
        newCardPos.transform.parent = this.transform;
        newCardPos.transform.localScale = new Vector3(1, 1, 1);

        cardPosClass.CardPos = newCardPos;
        cardPosClass.card = drawCard;
        cardPosClass.CardIndexOnHand = CardIndexOnHand;

        newCardPos.GetComponent<CardPlayController>().cardPos = cardPosClass;

        CardIndexOnHand += 1;
        Debug.Log(CardIndexOnHand);
        cardOnHand.Add(cardPosClass);
    }

    public void PlayCard(int index) {
        //play card will be called in CardPlayController
        foreach(CardPosClass pos in cardOnHand)
        {
            //can only play the card on hand with same "onhand" index 
            if(pos.CardIndexOnHand == index)
            {
                Destroy(pos.CardPos);
                discardPile.Enqueue(pos.card);
                CardCount -= 1;
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
