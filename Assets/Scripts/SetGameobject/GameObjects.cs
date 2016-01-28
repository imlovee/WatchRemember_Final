using UnityEngine;
using System.Collections;

/// <summary>
/// 전체 게임 오브젝트 - atlas, font설정
/// </summary>
public class GameObjects : MonoBehaviour
{
    public PackageManager pack;

    public ObjectGroup[] objectGroup;

//    public PopupWindows popupWindows;
//    public GamePopups gamePopups;
//    public Tutorials tutorials;
//    public Effects effects;
//    public GamePages gamePages;
//    public PageMains pageMains;
//    public PageQuestions pageQuestions;

    public delegate void SetWidget(bool isEnd);
    public event SetWidget setWidget;

    void Awake()
    {
        this.pack = PackageManager.Instance;
//        this.objectGroup = GetComponentsInChildren<ObjectGroup>();

        this.pack.onLoaded += pack_onLoaded;
    }

    void pack_onLoaded(bool loaded)
    {
        for (int i = 0; i < objectGroup.Length; i++)
        {
            if (!objectGroup[i].SetWidget(this.pack))
            {
                Debug.LogError("SetWidget Error: " + objectGroup[i].gameObject.name);
                SetWidgetEvent(false);
                return;
            }
        }
		Debug.Log ("SetLoad");
        SetWidgetEvent(true);
    }

    public void SetWidgetEvent(bool isEnd)
    {
        if (this.setWidget != null)
        {
            this.setWidget(isEnd);
        }
    }

}
