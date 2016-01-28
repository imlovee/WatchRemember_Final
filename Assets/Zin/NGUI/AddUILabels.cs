using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UILabel))]
public class AddUILabels : AddUIWidgets
{
    void Start()
    {
    }

    public UILabel Add(string text)
    {
        GameObject lebelGo = InstantiateWidget();

        UILabel label = lebelGo.GetComponent<UILabel>();
        label.text = text;

        Labels.Add(label);
        Send();

        return label;
    }
}
