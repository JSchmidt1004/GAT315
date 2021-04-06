using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class TextUI : MonoBehaviour
{
    public TMP_Text text = null;
    public StringData data = null;


    private void OnValidate()
    {
        if (data != null)
        {
            name = data.name;
            //label.text = name;
        }
        //slider.minValue = min;
        //slider.maxValue = max;
    }

    private void Start()
    {
        //slider.onValueChanged.AddListener(UpdateValue);
    }

    void Update()
    {
        text.text = data.value;
    }

    void UpdateValue(string value)
    {
        data.value = value;
        text.text = value.ToString();
    }
}