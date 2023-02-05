using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderFloat : MonoBehaviour
{
    public Slider slider;

    void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    public void ValueUpdate(float value)
    {
        Debug.Log("currentHP: " + value);
        slider.value = value;
    }

}