using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "card", menuName = "Cards")]
public class Card : ScriptableObject
{

    public int CardID;

    public string type;
    public int nutrition;
    public int waterCost;

    [TextArea]
    public string description;

    public Sprite image;
    public Sprite card;

}
