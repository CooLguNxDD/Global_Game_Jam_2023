using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using DG.Tweening;
public class CardManager : MonoBehaviour
{
    // full deck
    public List<Card> fullDeck = new();

    // current cards can be drawed
    public Queue<Card> DrawPile = new();

    //discard pile for special deck build
    public Queue<Card> DiscardPile = new();

    // the cards that in player's hand
    public List<CardPosClass> CardOnHand = new();

    public int maxCardOnHand = 5;
    public GameObject cardDisplayPrefab;

    public GameObject[] cardPresetPosition;

    // index to check each unique card draw to player' hands
    //start from 0 to infinite
    private int _cardIndexOnHand; 
    private int _cardCount;

    public void Awake()
    {
        _cardIndexOnHand = 0;
        _cardCount = 0;
    }
    public void Shuffle()
    {
        List<Card> tmp = fullDeck.ToList();
        
        Queue<Card> tmpCurrentCards = new Queue<Card>();
        System.Random r = new System.Random();
        while(tmp.Count > 0)
        {
            int index = r.Next(0, tmp.Count);
            tmpCurrentCards.Enqueue(tmp[index]);
            tmp.RemoveAt(index);
        }
        this.DrawPile = tmpCurrentCards;
    }

    public void DrawCard()
    {
        // shuffle when no more card
        if (DrawPile.Count == 0) Shuffle();
        
        if (_cardCount < maxCardOnHand)
        {
            // dequeue when draw a card
            Card drawCard = DrawPile.Dequeue();
            DisplayCardOnHand(drawCard);
            _cardCount++;
        }
    }

    public void DisplayCardOnHand(Card drawCard)
    {
        // spawn a new UI position to display card!

        GameObject newCardPos = Instantiate(cardDisplayPrefab, this.transform, true);
        newCardPos.GetComponent<CardDisplaySetting>().card = drawCard;
        newCardPos.transform.localScale = Vector3.one;
        
        //initial the storage class
        CardPosClass cardPosClass = new CardPosClass();
        cardPosClass.CardPos = newCardPos;
        cardPosClass.card = drawCard;
        cardPosClass.CardIndexOnHand = _cardIndexOnHand;
        
        //storage info into CardPlayController
        newCardPos.GetComponent<CardPlayController>().cardPosClass = cardPosClass;
        
        // add card info to hand
        CardOnHand.Add(cardPosClass);
        
        //set start animation
        Debug.Log(_cardCount);
        Debug.Log(cardPresetPosition[_cardCount].GetComponent<RectTransform>().position);
        
        
        newCardPos.GetComponent<CardDisplaySetting>().SetAlpha(0);
        StartCoroutine(cardDrawAnimation(cardPresetPosition[_cardCount].GetComponent<RectTransform>().position, newCardPos));
        _cardIndexOnHand += 1;
    }

    IEnumerator cardDrawAnimation(Vector3 toPos, GameObject card)
    {
        yield return new WaitForSeconds(0.05f);
        card.GetComponent<CardDisplaySetting>().SetAlpha(1);
        RectTransform CardTransform = card.transform.GetComponent<RectTransform>();
        CardTransform.position = new Vector3(2000, 0, 0);
        CardTransform.DOMove(toPos, 0.5f);
        yield return null;

    }

    public void PlayCard(int index) {

        //play card will be called in CardPlayController
        int cardListIndex = 0;
        foreach(CardPosClass pos in CardOnHand)
        {
            //can only play the card on hand with same "onhand" index 
            if(pos.CardIndexOnHand == index)
            {
                pos.CardPos.SetActive(false);
                DiscardPile.Enqueue(pos.card);
                _cardCount -= 1;

                //error here????
                CardOnHand.RemoveAt(cardListIndex);
                return;
            }
            cardListIndex++;
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
