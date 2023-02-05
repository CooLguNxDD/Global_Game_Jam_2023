using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUIPosY : MonoBehaviour
{
    public void Set(float value)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, value);
    }
}
