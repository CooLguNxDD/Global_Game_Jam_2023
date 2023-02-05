using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverable : MonoBehaviour
{

    private Color _originalColor;
    // Start is called before the first frame update
    public void Start()
    {
        _originalColor = transform.GetComponent<SpriteRenderer>().color;
    }

    private void OnMouseEnter()
    {
        if (Global.instance.draggingCard && transform.GetComponent<Tile>().isBuildAble)
        {
            // Debug.Log("enter");
            Global.instance.isValidLocation = true;
            Global.instance.buildOn = gameObject;
            transform.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    private void OnMouseExit()
    {
        if (Global.instance.draggingCard)
        {
            // Debug.Log("exit");
            Global.instance.isValidLocation = false;
            transform.GetComponent<SpriteRenderer>().color = _originalColor;
        }
    }
}
