using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class NGUIScore : ZinBehaviour
{
    public UIWidget m_title;
    public UILabel m_lbl_score;

    private int m_score = -1;
    public int Score { get { return m_score; } }

    public int m_startScore = 0;
    public float m_startDelay = 0;
    public string m_prefix = "";
    public bool useSound = true;
    private int AniPoint = 10;

    /// <summary>
    /// 올라가는 점수 폭에 따른 가중치(숫자가 작을수록 카운트 올라가는 속도가 빠르다)
    /// </summary>
    public int weight = 50;

    void Awake()
    {
        m_lbl_score = GetComponent<UILabel>();

        InitScore();
    }

    public void InitScore()
    {
        SetScoreNumber(m_startScore);

        SetScore(m_score);
    }

    public void SetScore(int score)
    {
        SetScoreNumber(score);

        SetText(score.ToString());
    }

    public void SetScoreNumber(int score)
    {
        m_score = score;
    }

    public void SetScoreAndRun(int score)
    {
        StartCoroutine(RunScoreAni(score));
    }

    public void SetScoreAndRun(int score, GameObject receive, string functionName)
    {
        m_notify = receive;
        m_functionName = functionName;
        SetScoreAndRun(score);
    }

    /// <summary>
    /// 자리수에 따른 애니메이션 효과 시점 
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    private int GetAniPoint(int score)
    {
        

        int m = score - m_score;
        int w = weight > m ? m : weight;

        return (int)(m / w);
    }

    IEnumerator RunScoreAni(int score)
    {
        if (score != 0)
        {
            yield return new WaitForSeconds(m_startDelay);

            int currentScore = m_score - 1;

            AniPoint = GetAniPoint(score);
            //Debug.LogWarning("ANI: " + AniPoint);

            //int currentScore = m_score;
            while (currentScore < score)
            {
                currentScore++;
                SetText(currentScore.ToString());
                if (currentScore % AniPoint == 0)
                {
                    if (useSound)
                    {
                        SoundFX.Instance.PlaySound(SoundType.COUNT_NUMBER);
                    }
                    yield return new WaitForFixedUpdate();
                }
            }
        }
        else
        {
            SetText("0");
        }

        m_score = score;
        Send(m_score);
    }

    public void SetScore(float score)
    {
        int iScore = (int)score;

        SetScore(iScore);
    }

    private void SetText(string text)
    {
        if (m_lbl_score != null)
        {
            m_lbl_score.text = string.Format("{0}{1}", m_prefix, text);
        }
        else
        {
            Debug.LogError(gameObject);
        }
    }


    public void Show()
    {
        SetActive(m_title, true);
        SetActive(m_lbl_score, true);
    }

    public void Hide()
    {
        SetActive(m_title, false);
        SetActive(m_lbl_score, false);

    }

    private void SetActive(UIWidget lbl, bool enable)
    {
        if (lbl != null)
        {
            lbl.enabled = enable;
        }
    }
}
