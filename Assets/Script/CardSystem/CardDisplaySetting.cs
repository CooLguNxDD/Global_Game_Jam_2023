using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplaySetting : MonoBehaviour
{

    public Card card;
    public CanvasGroup group;
    public CanvasGroup infoGroup;
    


    //front
    public Image CardBack;
    public Image CardBackground;
    public Image Creature;

    public Image BacksideShadow;
    

    public TMP_Text Flag_Text;

    public TMP_Text NutritionCost;
    public TMP_Text WaterCost;

    public TMP_Text Description;

    // Start is called before the first frame update
    public void SetAlpha(float alpha)
    {
        group.alpha = alpha;
    }

    public void SetOnDragging(bool dragging)
    {
        CardBackground.GetComponent<Image>().enabled = !dragging;
        BacksideShadow.GetComponent<Image>().enabled = !dragging;
        CardBack.GetComponent<Image>().enabled = !dragging;
        if (dragging) infoGroup.alpha = 0;
        else infoGroup.alpha = 1;

    }
    void Start()
    {
        if (card)
        {
            //Debug.Log(card.Name);
            Flag_Text.text = card.Name;
            NutritionCost.text = card.NutritionCost.ToString();
            WaterCost.text = card.WaterCost.ToString();
            Description.text = card.Description;

            Creature.sprite = card.CardImage;
        }
    }
}
