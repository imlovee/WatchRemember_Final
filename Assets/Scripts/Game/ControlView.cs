using UnityEngine;
using System.Collections;

public enum PageType
{
	NONE = -1,
	QUESTION = 0,
	MAIN
}

public enum StageType
{
	NONE = -1,				// 초기값
	FIRST,					// 시작 
	NORMAL,					// 일반
	CONTINUE_FIRST,			// 이어하기 시작 
	LAST					// 마지막 
}

public enum StageState
{
	NONE = -1,
	LEVEL_UP,
	GAMEOVER,
	GAMECLEAR
}

public class ControlView : MonoBehaviour
{
	public static ControlView Instance;

	private readonly int CONTINUE_COUNT = 3;
	private int remainCount = -1;

	public GameState m_state;

	public PageFlip m_pageFlip;

	public ZinWindow m_gameOverWindow;
	public ZinWindow m_gameClearWindow;
	public ZinWindow m_creditWindow;
	public ZinWindow m_setupWindow;
	public ZinWindow m_recordWindow;

	public SetWRPage m_setStage;

	public WRPageQuestionUp m_pageQPrefab_up;
	public WRPageQuestionDown m_pageQPrefab_down;
	public WRPageMainUp m_pageMPrefab_up;
	public WRPageMainDown m_pageMPrefab_down;

	public GameObjects m_gameObjects;

	public GameObject m_currentPageUp;
	public GameObject m_currentPageDown;
	public Transform m_pageUpParent;
	public Transform m_pageDownParent;

	public AdmobView m_admob;
	public ControlListScroll controlpackScroll;

	private int m_lastPageNo = -1;
	public static StageType CurrentStageType = StageType.NONE;
	public static StageState CurrentStageState = StageState.NONE;

	public delegate void ChagnePage (PageType page);

	public event ChagnePage changePage;

	public delegate void InitContorolView ();

	public event InitContorolView onInit;

	public int totalScore = -1;


	void Awake ()
	{
		Instance = this;
	}


	void Start ()
	{
		SoundFX.eventSoundState += new SoundFX.EventSoundState (PlaySoundState);
		m_gameObjects.setWidget += M_gameObjects_setWidget;
	}

	void M_gameObjects_setWidget (bool isEnd)
	{
		if (isEnd) {
			ShowMain ();
		}
	}


	private void SetChagePage (PageType pageType)
	{
		if (changePage != null) {
			changePage (pageType);
		}
	}

	private void SetInit ()
	{
		if (onInit != null) {
			onInit ();
		}
	}


	private void SetStageState ()
	{
		if (CurrentStageType == StageType.NONE) {
			CurrentStageType = StageType.FIRST;
		} else if (CurrentStageType == StageType.FIRST || CurrentStageType == StageType.CONTINUE_FIRST) {
			CurrentStageType = StageType.NORMAL;
		}

		if (CurrentStageState == StageState.GAMEOVER || CurrentStageState == StageState.LEVEL_UP) {
			CurrentStageType = StageType.CONTINUE_FIRST;
			CurrentStageState = StageState.NONE;
		}
	}

	/// <summary>
	/// 다음 페이지
	/// </summary>
	public void NextQuestion ()
	{
		ControlQuestionPage qp = m_currentPageUp.GetComponent<ControlQuestionPage> ();
		qp.NextQuestion ();
	}


	public void ShowQuestion ()
	{
		Debug.Log ("ShowQuestion");
		SetStageState ();
		m_pageFlip.SetPrevPage ();

		m_currentPageUp = newPage (m_pageQPrefab_up.gameObject, m_pageUpParent);
		m_currentPageDown = newPage (m_pageQPrefab_down.gameObject, m_pageDownParent);
		SetChagePage (PageType.QUESTION);

	}

	public void ShowQuestion (int score)
	{
		ShowQuestion ();
		WRPageQuestionUp qUp = m_currentPageUp.GetComponent<WRPageQuestionUp> ();
		qUp.SetScore (score);
	}

	public void ContinueQuestion ()
	{
		ZinWindow.OpendWindow.Hide ();

		m_setStage.SetContinueStageNumber ();

		ShowQuestion ();
		WRPageQuestionUp qUp = m_currentPageUp.GetComponent<WRPageQuestionUp> ();
		qUp.SetScore (totalScore);
	}

	public void ShowGameClear(int tScore) {
		totalScore = tScore;
		CurrentStageState = StageState.GAMECLEAR;

		SoundFX.Instance.PlaySound (SoundType.GAME_CLEAR);

		WindowGameClear window = m_gameClearWindow.GetComponent<WindowGameClear> ();
		window.SetTotalScore (totalScore);

		int bestScore = PlayerPrefs.GetInt (NameManager.PREF_BEST_SCORE, NameManager.PREF_BEST_SCORE_DEFAULT);
		window.SetBestScore (bestScore);
		window.SetCorrectImg (SetWRPage.Instance.m_correct);
		window.SetCashPoint (GameCash.CashPoint);

		int lastLevel = PlayerPrefs.GetInt (NameManager.PREF_LAST_LEVEL, NameManager.PREF_LAST_LEVEL_DEFAULT);
		SocialManager.Instance.Save (bestScore, lastLevel);

		m_gameClearWindow.Show ();
	}


	public void ShowGameOver (int tScore)
	{
		totalScore = tScore;
		CurrentStageState = StageState.GAMEOVER;

		remainCount--;
		SoundFX.Instance.PlaySound (SoundType.GAME_OVER);

		WindowGameOver window = m_gameOverWindow.GetComponent<WindowGameOver> ();
		window.SetTotalScore (totalScore);

		// 횟수 제한2회 and 인터넷연결 ok or 광고결제 ok
		if (remainCount > 0 && (Application.internetReachability != NetworkReachability.NotReachable || !ControlAd.m_useAd)) {
			window.ShowContinueBtn ();
		} else {
			window.HideContinueBtn ();
		}

		int bestScore = PlayerPrefs.GetInt (NameManager.PREF_BEST_SCORE, NameManager.PREF_BEST_SCORE_DEFAULT);
		window.SetBestScore (bestScore);
		window.SetCorrectImg (SetWRPage.Instance.m_correct);
		window.SetCashPoint (GameCash.CashPoint);

		m_gameOverWindow.Show ();

		int lastLevel = PlayerPrefs.GetInt (NameManager.PREF_LAST_LEVEL, NameManager.PREF_LAST_LEVEL_DEFAULT);
		SocialManager.Instance.Save (bestScore, lastLevel);
	}

	private void PlaySoundState (bool isEnd, SoundType type)
	{
		if (type == SoundType.GAME_OVER && isEnd) {
			m_admob.ShowView ();
		}
	}

	/// <summary>
	/// 페이지 값 불러오기 완료 시점
	/// </summary>
	public void SetStaged (PageType pageType)
	{
		if (m_currentPageUp == null || m_currentPageDown == null)
			return;

		m_pageFlip.Flip (pageType, m_currentPageUp, m_currentPageDown);
	}

	/// <summary>
	/// 페이지 넘기기 효과 완료 시점
	/// </summary>
	/// <param name="pageType"></param>
	void EndFlip (PageType pageType)
	{
		switch (pageType) {
		case PageType.NONE:
			break;

		case PageType.QUESTION:
			if (!TutorialManager.Instance.PlayTutorial ()) {
				RunQuestionTween ();
			}

			break;

		case PageType.MAIN:


			break;

		default:
			break;
		}
	}

	public void RunQuestionTween ()
	{
		ControlQuestionPage controlQ = m_currentPageUp.GetComponent<ControlQuestionPage> ();
		controlQ.RunQuestionTween ();
	}

	private GameObject newPage (GameObject go, Transform parent)
	{
		m_lastPageNo++;

		GameObject newGo = GameObject.Instantiate (go) as GameObject;
		UIPanel copyObj = newGo.GetComponent<UIPanel> ();
		copyObj.transform.parent = parent;
		copyObj.transform.localScale = new Vector3 (1, 0, 1);
		copyObj.transform.localPosition = Vector3.zero;


		WRPage page = newGo.GetComponent<WRPage> ();
		page.m_isUse = true;
		//page.SetPageNumber(m_lastPageNo);

		return newGo;
	}


	public void Reset ()
	{
		if (ZinWindow.OpendWindow != null && ZinWindow.OpendWindow.name == "Panel_packageList")
			return;
		
		Debug.Log ("Reset");
		remainCount = CONTINUE_COUNT;
		m_state.Play ();

		m_setStage.InitStageNumber ();

		ShowQuestion ();

		BGSound.Instance.PlaySound (BGSoundType.MAIN);
	}

	public void ShowMain ()
	{
		Debug.Log ("ShowMain");
		CurrentStageType = StageType.NONE;
		m_state.Ready ();

		m_pageFlip.SetPrevPage ();

		m_currentPageUp = newPage (m_pageMPrefab_up.gameObject, m_pageUpParent);
		m_currentPageDown = newPage (m_pageMPrefab_down.gameObject, m_pageDownParent);

		BGSound.Instance.Stop ();

		SetChagePage (PageType.MAIN);
	}
}

