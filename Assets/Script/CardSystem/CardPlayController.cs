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

    public Tile tile;
    
    private Vector3 _previousPosition;
    
    private CardDisplaySetting _cardDisplaySetting;
    private RectTransform _rectTransform;

    public void Start()
    {
        cardSystem = this.transform.GetComponentInParent<CardManager>();
        _cardDisplaySetting = CardPosClass.CardPos.GetComponent<CardDisplaySetting>();
        _rectTransform = transform.GetComponent<RectTransform>();

    }
    public void MouseDown() {
        //start dragging
        
        Global.draggingCard = CardPosClass.card;

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
        if (Global.Nutrition > CardPosClass.card.NutritionCost &&
            Global.Water > CardPosClass.card.WaterCost)
        {
            Global.Nutrition -= CardPosClass.card.NutritionCost; 
            Global.Water -= CardPosClass.card.WaterCost;
            Debug.Log(Global.Nutrition);
            Debug.Log(Global.Water);
            return true;
        }
        
        return false;
    }

    public void CardBackToEnd()
    {
        
        _rectTransform.DOMove(_previousPosition, 0.25f);
    }

    public void SetTowerScriptable(Transform newObject)
    {
        if (CardPosClass.card.scriptableObject && CardPosClass.card.type == Global.TileType.TOWER)
        {
            newObject.GetComponent<TowerSampleScript>().tower = CardPosClass.card.scriptableObject;
        }
    }

    public void MouseUp()
    {
        //start dragging
        
        if (Global.isValidLocation && Global.draggingCard)
        {
            if (ValidCost())
            {
                Debug.Log("played a card");
                cardSystem.PlayCard(CardPosClass.CardIndexOnHand);

                Tile source = Global.buildOn.GetComponent<Tile>();
                (int, int) xy = (source.x, source.y);
                Vector3 pos = Global.buildOn.transform.position;
                
                Destroy(TileManager.instance.board_pieces[xy.Item1, xy.Item2].gameObject);
                
                TileManager.instance.board_pieces[xy.Item1, xy.Item2] = Instantiate(CardPosClass.card.spwanableObject, 
                    pos, Quaternion.identity,
                    TileManager.instance.parent.transform
                    
                ).transform;
                
                Transform thisObject = TileManager.instance.board_pieces[xy.Item1, xy.Item2];
                pos = thisObject.position;
                thisObject.position = new Vector3(pos.x, pos.y, 100f);
                
                TileManager.instance.board[xy.Item1, xy.Item2] = (int)source.type;
                thisObject.Find("Square").GetComponent<Tile>().setXY(xy.Item1, xy.Item2);
                thisObject.Find("Square").GetComponent<Tile>().isBuildAble = false;
                TileManager.instance.updateNeighborBuildableAt(xy.Item1, xy.Item2);
                source.type = CardPosClass.card.type;
                
                SetTowerScriptable(TileManager.instance.board_pieces[xy.Item1, xy.Item2]);
                return;
            }
            Debug.Log("not enough cost");
        }
        _rectTransform.localScale = Vector3.one;
        _cardDisplaySetting.SetAlpha(1f);
        _cardDisplaySetting.SetOnDragging(false);
        
        CardBackToEnd();
        //set dragging to false
        Global.draggingCard = null;
        //Debug.Log("up");
    }
}
