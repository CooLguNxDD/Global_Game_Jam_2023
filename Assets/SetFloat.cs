using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetFloat : MonoBehaviour
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
        slider.value = value;
    }

}