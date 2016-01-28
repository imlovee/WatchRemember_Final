using UnityEngine;
using System.Collections;

public class WindowRecord : MonoBehaviour
{
    public UIButton leaderBoard;
    public UIButton AchieveMent;

    void Start()
    {
        GameObject leaderGo = ZinUtil.GetChildObj(gameObject, "Sprite_btn_leaderboard");
        GameObject achieveGo = ZinUtil.GetChildObj(gameObject, "Sprite_btn_Achievement");

        leaderBoard = leaderGo.GetComponent<UIButton>();
        AchieveMent = achieveGo.GetComponent<UIButton>();


        leaderBoard.onClick.Add(new EventDelegate(SocialManager.Instance, "ShowLeaderBoard"));
        AchieveMent.onClick.Add(new EventDelegate(SocialManager.Instance, "ShowAchievem"));
    }
}
