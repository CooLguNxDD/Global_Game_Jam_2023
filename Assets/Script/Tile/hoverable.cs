using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverable : MonoBehaviour
{
    // Start is called before the first frame update
    public Global global;
    private void Awake()
    {
        if (global == null) global = transform.GetComponentInParent<Global>();
    }
    private void OnMouseEnter()
    {
        if (global.draggingCard)
        {
            // Debug.Log("enter");
            global.isValidLocation = true;
            transform.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    private void OnMouseExit()
    {
        if (global.draggingCard)
        {
            // Debug.Log("exit");
            global.isValidLocation = false;
            transform.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
