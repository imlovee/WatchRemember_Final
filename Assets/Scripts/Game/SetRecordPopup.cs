using UnityEngine;
using System.Collections;

public class SetRecordPopup : MonoBehaviour
{
    public SocialManager m_gp;
    public UIButton m_btn_leaderBoard;
    public UIButton m_btn_Achievem;


    void Start()
    {
        m_gp = SocialManager.Instance;

        m_btn_leaderBoard.onClick.Add(new EventDelegate(m_gp, "ShowLeaderBoard"));
        m_btn_Achievem.onClick.Add(new EventDelegate(m_gp, "ShowAchievem"));
        
    }

}
