using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddUITextures : AddUIWidgets
{
    void Start()
    {
    }

    public void Add(Texture texture)
    {
        GameObject lebelGo = InstantiateWidget();

        UITexture label = lebelGo.GetComponent<UITexture>();
        label.mainTexture = texture;

        Labels.Add(label);
    }
}
