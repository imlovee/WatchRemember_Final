using UnityEngine;
using System.Collections;

public class ItemClick : ZinBehaviours
{
	public int index;
	public static int SelectIndex = 0;

    void Start()
    {
        index = 0;

        foreach (Transform child in this.transform.parent)
        {
            if (child == transform)
            {
                break;
            }
            index++;
        }

		Send ();
    }

    void OnClick()
    {
        Debug.Log(index + "번 째 아이템 클릭됨");
		ItemClick.SelectIndex = this.index;
    }

	public void SetClick() {
		OnClick ();
	}
}