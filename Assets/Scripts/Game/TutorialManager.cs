using UnityEngine;
using System.Collections;

public class TutorialManager : ZinBehaviour
{
    private static TutorialManager instance;
    public static TutorialManager Instance
    {
        get
        { return TutorialManager.instance; }
    }

    public delegate void PlayingTutorial(bool isPlaying);
    public event PlayingTutorial playingTutorial;


    public ZinWindow[] guide_panels;

    /// <summary>
    /// 튜토리얼 진행중 여부
    /// </summary>
    public bool isPlaying = false;
    public int CurrentTuto = 0;

    /// <summary>
    /// 튜토리얼 플레이 여부
    /// </summary>
    public bool isPlay = false;


    void Awake()
    {
        transform.localPosition = Vector3.zero;
        instance = this;

        int playTuto = PlayerPrefs.GetInt(NameManager.PREF_PLAY_TUTORIAL, NameManager.PREF_PLAY_TUTORIAL_DEFAULT);
//        int playTuto = 0;
        isPlay = playTuto == 0 ? false : true;

        CurrentTuto = 0;

        
    }

    void Instance_onLoaded(bool loaded)
    {
        if (loaded)
        {
            for (int i = 0; i < guide_panels.Length; i++)
            {
                guide_panels[i].Hide();
                //guide_panels[i].SetSendMethodName("OpendMainGuideWindow", "CloseMainGuideWindow");
            }

            StartCoroutine(StartTuto());
        }
    }

    /// <summary>
    /// 정답위치로 이동하기
    /// </summary>
    /// <param name="pos"></param>
    public void SetAnswer(Vector3 pos)
    {
        int answerIndex = 2;
        if (guide_panels.Length <= answerIndex) return;

        SetTutorialWindow tuto = guide_panels[answerIndex].GetComponentInChildren<SetTutorialWindow>();
        if (tuto != null)
        {
            tuto.SetPanel(0, pos);
        }
    }

    void Start()
    {
        PackageManager.Instance.onLoaded += Instance_onLoaded;

    }

    void OnDestory()
    {
        PackageManager.Instance.onLoaded -= Instance_onLoaded;     

    }

    IEnumerator StartTuto()
    {
        yield return SetDelay(0.2f);
        PlayTutorial();
    }


    public void CloseWindow()
    {
        for (int i = 0; i < guide_panels.Length; i++)
        {
            guide_panels[i].Hide();
        }
    }

    public void RunTotorial()
    {
        isPlay = false;
        CurrentTuto = 0;

        PlayTutorial();
    }

    IEnumerator SetDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    /// <summary>
    /// 튜토리얼 진행
    /// </summary>
    /// <returns>튜토리얼 실행 여부</returns>
    public bool PlayTutorial()
    {
        if (!isPlay)
        {
            if (CurrentTuto == 0)
            {
                StartTutorial();
            }
            else 
            {
                NextTutorial();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 튜토리얼 시작
    /// </summary>
    public void StartTutorial()
    {
        isPlaying = true;
        SetPlayingTuto(isPlaying);

        NextTutorial();

    }

    /// <summary>
    /// 튜토리얼 이벤트 발생
    /// </summary>
    /// <param name="isPlaying"></param>
    private void SetPlayingTuto(bool isPlaying)
    {
        if (playingTutorial != null)
        {
            playingTutorial(isPlaying);
        }
    }

    /// <summary>
    /// 종료 이벤트 발생
    /// </summary>
    void SetEndPlaying()
    {
        SetPlayingTuto(false);
    }



    /// <summary>
    /// 튜토리얼 종료
    /// </summary>
    public void EndTutorial()
    {
        isPlaying = false;

        for (int i = 0; i < guide_panels.Length; i++)
        {
            //guide_panels[i].Hide();
            //BlinkSprite blink = guide_panels[i].GetComponent<BlinkSprite>();
            //if (blink != null)
            //{
            //    blink.StopBlink();
            //}
        }

        isPlay = true;
        GameState.m_state = PlayState.PLAY;
        PlayerPrefs.SetInt(NameManager.PREF_PLAY_TUTORIAL, isPlay ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 다음 튜토리얼
    /// </summary>
    public void NextTutorial()
    {
        if (guide_panels == null) return;
        if (!isPlaying) return;

        GameState.m_state = PlayState.PAUSE;

        if (CurrentTuto > 0)
        {
            guide_panels[CurrentTuto - 1].Hide();
        }

        guide_panels[CurrentTuto].Show();

        CurrentTuto++;

        if (CurrentTuto >= guide_panels.Length)
        {
            EndTutorial();
        }
    }

    public void SetTutorialPosition(Vector3 pos)
    {
        for (int i = 0; i < guide_panels.Length; i++)
        {
            guide_panels[i].transform.localPosition = pos;
        }
    }
}

