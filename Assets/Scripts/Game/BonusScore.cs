using UnityEngine;
using System.Collections;

public class BonusScore : ZinBehaviour
{
    public NGUIScore m_score;
    public UISprite m_bg;
    public string m_prefix;


    void Start()
    {
        m_score.m_prefix = m_prefix;
        m_score.m_notify = gameObject;
        m_score.m_functionName = "EndScoreAni";
        m_score.useSound = false;
        Hide();
    }

    public void Show(int score, string prefix)
    {
        if (score > 0)
        {
            ShowBG(true);
            m_score.SetScore(0);
            m_score.Show();
            //m_score.SetScore(score);
            m_score.m_prefix = prefix;
            m_score.SetScoreAndRun(score);
        }
    }

    public void Hide()
    {
        ShowBG(false);
        m_score.Hide();
    }

    public IEnumerator Hide(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hide();
    }

    private void ShowBG(bool isShow)
    {
        if (m_bg != null)
        {
            m_bg.enabled = isShow;
        }
    }

    public void EndScoreAni()
    {
        Send();
    }

}
