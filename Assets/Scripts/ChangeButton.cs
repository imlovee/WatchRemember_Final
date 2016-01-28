using UnityEngine;
using System.Collections;


public enum ButtonState
{
    NONE = -1,
    BUY = 0,
    DOWNLOAD = 1,
    PLAY = 2
}

public class ChangeButton : MonoBehaviour
{
    public UIPanel[] btn;
    public int Index = 0;

    public ButtonState state = ButtonState.BUY;

    void Awake()
    {
        SetButtons(Index);
    }

    public void Change()
    {
        if (btn == null) return;
        if (Index >= btn.Length)
        {
            Index = 0;
        }
        else
        {
            Index++;
        }

        SetButtons(Index);
    }

    public void SetButtons(ButtonState state)
    {
        SetButtons((int)state);
		this.state = state;
    }

    private void SetButtons(int index)
    {
        for (int i = 0; i < btn.Length; i++)
        {
            if (i == index)
            {
                btn[i].gameObject.SetActive(true);
            }
            else
            {
                btn[i].gameObject.SetActive(false);
            }
        }
    }
}
