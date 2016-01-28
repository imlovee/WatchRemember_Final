using UnityEngine;
using System.Collections;

public class SetButtons : MonoBehaviour
{
    public UIButton[] Btns;

    public Vector3[] StartPositions;
    public Vector3[] MovePositions;



    void Start()
    {
        SetStartPosition();
    }

    public void SetStartPosition()
    {
        SetPosition(StartPositions);
    }

    private void SetPosition(Vector3[] posistions)
    {
        if (Btns == null) return;

        for (int i = 0; i < Btns.Length; i++)
        {
            Btns[i].transform.localPosition = posistions[i];
        }
    }

    public void SetMovePosition()
    {
        SetPosition(MovePositions);
    }

}
