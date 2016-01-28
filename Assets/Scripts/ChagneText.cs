using UnityEngine;
using System.Collections;

public class ChagneText : MonoBehaviour
{
    private UILabel lbl;
    public string text1;
    public string text2;


    void Awake()
    {
        lbl = GetComponent<UILabel>();
    }

    void Start()
    {
        Change();
    }

    public void Change()
    {
        if (lbl == null) return;

        if (lbl.text == text1)
        {
            lbl.text = text2;
        }
        else
        {
            lbl.text = text1;
        }
        
        
    }

}
