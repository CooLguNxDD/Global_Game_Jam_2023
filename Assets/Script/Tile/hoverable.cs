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
        if (Global.draggingCard)
        {
            // Debug.Log("enter");
            Global.isValidLocation = true;
            transform.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    private void OnMouseExit()
    {
        if (Global.draggingCard)
        {
            // Debug.Log("exit");
            Global.isValidLocation = false;
            transform.GetComponent<SpriteRenderer>().color = _originalColor;
        }
    }
}
