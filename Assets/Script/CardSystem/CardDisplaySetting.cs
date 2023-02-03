using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplaySetting : MonoBehaviour
{

    public Card card;

    //back image
    public Image Back;

    //front
    public Image CardBack;
    public Image CardBackground;
    public Image CardImage;

    public TMP_Text Flag_Text;

    public TMP_Text NutritionCost;
    public TMP_Text WaterCost;

    public TMP_Text Description;

    // Start is called before the first frame update
    void Start()
    {
        if (card)
        {
            Debug.Log(card.Name);
            Flag_Text.text = card.Name;
            NutritionCost.text = card.NutritionCost.ToString();
            WaterCost.text = card.NutritionCost.ToString();
            Description.text = card.Description;

            CardImage.sprite = card.CardImage;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
