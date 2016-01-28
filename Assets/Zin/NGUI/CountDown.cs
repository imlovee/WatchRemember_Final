using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class CountDown : ZinBehaviour
{
    public int m_startCount = 5;
    public int m_currentCount = 0;

    private UILabel m_label;

    private float m_time = -1;
    public bool m_isPlay = false;


    void Awake()
    {
        m_label = GetComponent<UILabel>();
    }

    void Start()
    {
        //Init();
    }

    void OnEnable()
    {
        //Init();
    }

    void Init()
    {
        m_time = m_startCount;
        m_currentCount = m_startCount;

        m_label.text = m_currentCount.ToString();
    }

    public void Init(int startCount, GameObject notify, string functionName)
    {
        m_startCount = startCount;
        m_notify = notify;
        m_functionName = functionName;

        Init();
    }

    /// <summary>
    /// 지난 카운트 개수 가져오기
    /// </summary>
    /// <returns></returns>
    public int GetCountTime()
    {
        return m_startCount - GetCount(m_time);
    }



    public void Play()
    {
        m_isPlay = true;
    }

    private int GetCount(float time)
    {
        return Mathf.CeilToInt(time); 
    }

    void Update()
    {
        if (!m_isPlay) return;
        if (GameState.m_state == PlayState.PAUSE) return;

        m_time -= Time.deltaTime;

        int iTime = GetCount(m_time);
        if (iTime < m_currentCount)
        {
            m_currentCount = iTime;
            m_label.text = m_currentCount.ToString();
            SoundFX.Instance.PlaySound(SoundType.COUNT);

            if (m_currentCount <= 0)
            {
                Stop();
            }
        }
    }

    public void Stop()
    {
        m_isPlay = false;
        Send();
    }
}
