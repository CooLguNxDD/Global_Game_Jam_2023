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
    public List<Card> cardOnHand = new List<Card>();

    public int MaxCardOnHand = 5;
    public GameObject CardDisplayPrefab;

    private int CardCount = 0;
    
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

        GameObject newPos = Instantiate(CardDisplayPrefab);
        cardOnHand.Add(drawCard);
        newPos.GetComponent<CardDisplaySetting>().card = drawCard;
        newPos.transform.parent = this.transform;
        newPos.transform.localScale = new Vector3(1,1,1);
    }

    public void PlayCard(int index) {

        discardPile.Enqueue(cardOnHand[index]);
        cardOnHand.RemoveAt(index);
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
