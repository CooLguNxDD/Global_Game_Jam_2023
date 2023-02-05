using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
public class CardPlayController : MonoBehaviour
{
    // Start is called before the first frame update
    public CardManager cardSystem;
    public CardPosClass cardPosClass;

    public Tile tile;
    
    private Vector3 _previousPosition;
    
    private CardDisplaySetting _cardDisplaySetting;
    private RectTransform _rectTransform;

    public void Start()
    {
        cardSystem = this.transform.GetComponentInParent<CardManager>();
        _cardDisplaySetting = cardPosClass.CardPos.GetComponent<CardDisplaySetting>();
        _rectTransform = transform.GetComponent<RectTransform>();

    }
    public void MouseDown() {
        //start dragging
        
        Global.instance.draggingCard = cardPosClass.card;

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
        if (Global.instance.Nutrition > cardPosClass.card.NutritionCost &&
            Global.instance.Water > cardPosClass.card.WaterCost)
        {
            Global.instance.SetNutrition(Global.instance.Nutrition - cardPosClass.card.NutritionCost); 
            Global.instance.SetWater(Global.instance.Water - cardPosClass.card.WaterCost);
            // Debug.Log(Global.Nutrition);
            // Debug.Log(Global.Water);
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
        if (cardPosClass.card.scriptableObject && cardPosClass.card.type == Global.TileType.TOWER)
        {
            newObject.GetComponent<TowerSampleScript>().tower = cardPosClass.card.scriptableObject;
        }
    }

    public void MouseUp()
    {
        //start dragging
        
        if (Global.instance.isValidLocation && Global.instance.draggingCard)
        {
            if (ValidCost())
            {
                Debug.Log("played a card");
                cardSystem.PlayCard(cardPosClass.CardIndexOnHand);

                Tile source = Global.instance.buildOn.GetComponent<Tile>();
                (int, int) xy = (source.x, source.y);
                Vector3 pos = Global.instance.buildOn.transform.position;
                Global.instance.CalculateTileProfit(TileManager.instance.board_pieces[xy.Item1, xy.Item2].transform.Find("Square").GetComponent<Tile>().type, cardPosClass.card.spwanableObject.transform.Find("Square").GetComponent<Tile>().type);
                TileManager.instance.board[xy.Item1, xy.Item2] = (int)cardPosClass.card.spwanableObject.transform.Find("Square").GetComponent<Tile>().type;
                Destroy(TileManager.instance.board_pieces[xy.Item1, xy.Item2].gameObject);

                TileManager.instance.board_pieces[xy.Item1, xy.Item2] = Instantiate(cardPosClass.card.spwanableObject, 

                    pos, Quaternion.identity,
                    TileManager.instance.parent.transform
                    
                ).transform;
                
                Transform thisObject = TileManager.instance.board_pieces[xy.Item1, xy.Item2];
                pos = thisObject.position;
                thisObject.position = new Vector3(pos.x, pos.y, 100f);
                
                
                thisObject.Find("Square").GetComponent<Tile>().setXY(xy.Item1, xy.Item2);
                thisObject.Find("Square").GetComponent<Tile>().isBuildAble = false;
                thisObject.Find("Square").GetComponent<Tile>().UpdateArt();
                TileManager.instance.updateNeighborBuildableAt(xy.Item1, xy.Item2);
                List<Transform> neighbors = TileManager.instance.GetNeighborTile(xy.Item1, xy.Item2);
                for (int i=0; i<neighbors.Count;i++)
                {
                    neighbors[i].Find("Square").GetComponent<Tile>().UpdateArt();
                }
                // source.type = CardPosClass.card.type;
                
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
        Global.instance.draggingCard = null;
        //Debug.Log("up");
    }
}
