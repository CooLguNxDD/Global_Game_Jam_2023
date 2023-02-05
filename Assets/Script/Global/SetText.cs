using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetText : MonoBehaviour
{
    public TextMeshProUGUI _TMPtext;

    void Start()
    {
        if (_TMPtext == null)
        {
            _TMPtext = GetComponent<TextMeshProUGUI>();
        }
    }

    public void TextUpdate(string text)
    {
        _TMPtext.text = text;
    }

}