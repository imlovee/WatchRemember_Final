using UnityEngine;
using System.Collections;


public class TouchEvent : MonoBehaviour
{
    public delegate void OnTouchEvent(TouchPhase touchPase, int id, Vector2 pos, Vector2 deltaPos);
    public static event OnTouchEvent onTouchEvent;

    private Vector2[] startPos = new Vector2[5];
    private Vector2[] movedPos = new Vector2[5];

    void Update()
    {
        int count = Input.touchCount;
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            Touch touch = Input.GetTouch(i);
            int id = touch.fingerId;
            Vector2 pos = touch.position;
            if (touch.phase == TouchPhase.Began) startPos[id] = touch.position;
            
            if (touch.phase == TouchPhase.Began)
            {
                movedPos[id].x = movedPos[id].y = 0;
            }
            else
            {
                movedPos[id].x = pos.x - startPos[id].x;
                movedPos[id].y = pos.y - startPos[id].y;
            }

            if (onTouchEvent != null) onTouchEvent(touch.phase, id, pos, movedPos[id]);
        }
    }
    void Start()
    {
        onTouchEvent += onTouch;
    }

    void onTouch(TouchPhase touchPase, int id, Vector2 pos, Vector2 deltaPos)
    {
        //Log.log(string.Format("{0}: {1}, {2}, {3}", touchPase, id, pos, deltaPos));
        Debug.Log(string.Format("{0}: {1}, {2}, {3}", touchPase, id, pos, deltaPos));
    }
}
