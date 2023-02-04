using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayController : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnMouseDown()
    {
        Debug.Log("down");
        transform.position += Vector3.up * 5;
    }
}
