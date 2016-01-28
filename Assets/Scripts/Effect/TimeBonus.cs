using UnityEngine;
using System.Collections;

public class TimeBonus : ZinBehaviour
{
    public ZinTweenAlpha m_alpha;
    public ZinTweenScale m_scale;
    public NGUIScore m_score;
    public string m_prefix;

    void Start()
    {
        m_score.m_prefix = m_prefix;
        m_score.useSound = false;
        m_score.Hide();
        m_score.weight = 10;
        m_score.SetNotify(gameObject, "EndScoreAni");
    }

    public void Show(int score)
    {
        if (score > 0)
        {
            m_score.SetScore(0);
            m_score.Show();
            //m_score.SetScore(score);
            m_score.SetScoreAndRun(score);
        }
    }

    public void Hide()
    {
        m_score.Hide();
    }

    public void EndScoreAni()
    {
        Send();

        if (m_scale == null) return;
        //m_scale.InitTween();
        //m_scale.ShowTween(Vector3.one, Vector3.one * 2);

        if (m_alpha == null) return;
        //m_alpha.InitTween();
        //m_alpha.ShowTween(1, 0);

    }

}
