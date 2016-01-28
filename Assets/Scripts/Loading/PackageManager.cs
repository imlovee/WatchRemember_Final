using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PackageManager : MonoBehaviour
{
	private static PackageManager instance;

	public static PackageManager Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject ("PackageManager");
				instance = go.AddComponent<PackageManager> ();
			}

			return PackageManager.instance;
		}
	}


	public string packageName;

	GameDataManager dataManager;

	public UIPanel uiroot;


	public Texture2D[] Pictures;
	public AudioClip[] SoundClips;
	public AudioClip[] BgSoundClips;

	public UIAtlas commonAtlas;

	public Dictionary<string, UIAtlas> atlasList = new Dictionary<string, UIAtlas> ();
	public Dictionary<string, XMLObjectGroup> atlasXml = new Dictionary<string, XMLObjectGroup> ();
    
	public IDictionary<string, UIFont> fontList = new Dictionary<string, UIFont> ();
	public XMLFontGroup fontXml;

	public delegate void LoadedPackage (bool loaded);

	/// <summary>
	/// 패키지 로드 완료시점
	/// </summary>
	public event LoadedPackage onLoaded;

	public delegate void LoadedBundle (int totalCount,int loadCount,string name);

	/// <summary>
	/// 번들 로드 완료시점
	/// </summary>
	public event LoadedBundle onLoadedBundle;

	private bool isLoaded = true;
	private int loadCount = 0;


	void Awake ()
	{
		dataManager = GameDataManager.Instance;

		packageName = PlayerPrefs.GetString (NameManager.PREF_PLAY_PACKAGE, NameManager.PREF_PLAY_PACKAGE_DEFAULT);
		PackageListManager.Instance.onLoadXml += Instance_onLoadXml;
	}

	private void Instance_onLoadXml (bool loaded)
	{
		StartCoroutine (LoadPackage (packageName));
	}


	//private void SetPercent(float per)
	//{
	//    loadScreen.SetProgress(per);
	//}

	void Start ()
	{
		//yield return StartCoroutine(LoadPackage(packageName));
	}

	public IEnumerator LoadPackage (string packageName)
	{
		Debug.Log ("callLoadPack: " + packageName);

		PackageList list = PackageListManager.Instance.packList;

		Package pack = list.GetPackage (packageName);
		if (pack == null) {
			Debug.LogError ("package Not Found: " + packageName);
			yield return 0;
		}


		Array bundleTypeArr = Enum.GetValues (typeof(FileType));

		string[] packageFiles = PackageFileList.Instance.FileNames;
		//ZinBundle[] bundles = pack.Bundles;

		if (bundleTypeArr.Length != packageFiles.Length) {
			Debug.LogError ("File synch Error: ");
			yield return 0;
		}

		string bundleFileName = string.Empty;
		for (int i = 0; i < bundleTypeArr.Length; i++) {
			FileType type = (FileType)bundleTypeArr.GetValue (i);
			bundleFileName = packageFiles [i];
			//bundleFileName = bundles[i].BundleName;
			switch (type) {
			case FileType.Font:
				yield return StartCoroutine (LoadFont (bundleFileName, "Font.xml"));
				break;

			case FileType.BGSound:
				yield return StartCoroutine (LoadBGSounds (bundleFileName));
				break;

			case FileType.Common_pack:
				yield return StartCoroutine (LoadAtlas (bundleFileName, "Common_pack_Atlas", "Common_pack.xml"));
				break;

			case FileType.Effect:
				yield return StartCoroutine (LoadAtlas (bundleFileName, "Effect_Atlas", "Effect.xml"));
				break;

			case FileType.GameLevel:
				yield return StartCoroutine (LoadGameLevel (bundleFileName));
				break;

			case FileType.GamePopup:
				yield return StartCoroutine (LoadAtlas (bundleFileName, "GamePopup_Atlas", "GamePopup.xml"));
				break;

			case FileType.Language:
				yield return StartCoroutine (LoadLanguage (bundleFileName));
				break;

			case FileType.PageMain:
				yield return StartCoroutine (LoadAtlas (bundleFileName, "PageMain_Atlas", "PageMain.xml"));
				break;

			case FileType.PageQuestion:
				yield return StartCoroutine (LoadAtlas (bundleFileName, "PageQuestion_Atlas", "PageQuestion.xml"));
				break;

			case FileType.Picture:
				yield return StartCoroutine (LoadPictures (bundleFileName));
				break;

			case FileType.Popup:
				yield return StartCoroutine (LoadAtlas (bundleFileName, "Popup_Atlas", "Popup.xml"));
				break;

			case FileType.Sound:
				yield return StartCoroutine (LoadSounds (bundleFileName));
				break;

			case FileType.Tutorial:
				yield return StartCoroutine (LoadAtlas (bundleFileName, "Tutorial_Atlas", "Tutorial.xml"));
				break;

			default:
				break;
			}
			SetLoadedBundle (bundleTypeArr.Length, bundleFileName);
		}

		SetLoaded (isLoaded);
	}


	public void SetLoadedBundle (int totalCount, string name)
	{
		if (onLoadedBundle != null) {
			loadCount++;
			onLoadedBundle (totalCount, loadCount, name);

		}
	}

	/// <summary>
	/// 게임 레벨별 설정 불러오기
	/// </summary>
	/// <returns></returns>
	public IEnumerator LoadGameLevel (string fileName)
	{
		yield return StartCoroutine (dataManager.LoadXML (packageName, fileName));
		if (!string.IsNullOrEmpty (dataManager.xml)) {
			StageList.Instance.SetStageList (dataManager.xml);
			//LanguageManager.Instance.SetPackLanguage(dataManager.xml);
		} else {
			isLoaded = false;
		}
		dataManager.xml = string.Empty;
	}

	/// <summary>
	/// 언어 XML 불러오기
	/// </summary>
	/// <returns></returns>
	public IEnumerator LoadLanguage (string fileName)
	{
		yield return StartCoroutine (dataManager.LoadXML (packageName, fileName));
		if (!string.IsNullOrEmpty (dataManager.xml)) {
			LanguageManager.Instance.SetPackLanguage (dataManager.xml);
			//SetLoaded(true);
		} else {
			//SetLoaded(false);
			isLoaded = false;
		}
		dataManager.xml = string.Empty;
	}

	private void SetLoaded (bool loaded)
	{
		if (onLoaded != null) {
			onLoaded (loaded);
		}
	}

	private GameObject InstantiateGameObject (GameObject go)
	{
		GameObject newGo = GameObject.Instantiate (go) as GameObject;

		newGo.transform.parent = uiroot.transform;
		newGo.transform.localPosition = Vector3.zero;
		newGo.transform.localScale = Vector3.one;

		return newGo;
	}

	private IEnumerator LoadFont (string fileName, string xmlFileName)
	{
		// 폰트 xml 로드
		yield return StartCoroutine (dataManager.LoadXML (packageName, xmlFileName));

		if (dataManager.xml != null) {
			XMLFontGroup objGrp = ZinSerializerForXML.Deserialization<XMLFontGroup> (dataManager.xml);
			this.fontXml = objGrp;
		} else {
			this.isLoaded = false;
			yield return 0;
		}

		// 폰트 로드
		yield return StartCoroutine (dataManager.LoadGameObject (packageName, fileName, this.fontXml.FontNames));
		if (dataManager.loadObj != null) {
			GameObject[] objectList = dataManager.loadObj;
			for (int i = 0; i < objectList.Length; i++) {
				this.fontList.Add (objectList [i].name, objectList [i].GetComponent<UIFont> ());
			}
		} else {
			this.isLoaded = false;
		}
	}

	private IEnumerator LoadAtlas (string fileName, string prefabName, string xmlFileName)
	{

		// 아틀라스 xml 로드
		yield return StartCoroutine (dataManager.LoadXML (packageName, xmlFileName));

		string GameObjectName = string.Empty;
		if (dataManager.xml != null) {
			XMLObjectGroup objGrp = ZinSerializerForXML.Deserialization<XMLObjectGroup> (dataManager.xml);
			GameObjectName = objGrp.Name;
			atlasXml.Add (GameObjectName, objGrp);

		} else {
			this.isLoaded = false;
			yield return 0;
		}

		// 아틀라스 로드
		yield return StartCoroutine (dataManager.LoadGameObject (packageName, fileName, prefabName));
		if (dataManager.loadObj != null) {
			GameObject[] objectList = dataManager.loadObj;
			if (objectList == null)
				Debug.LogError ("Load File Not Found : " + fileName);
			
			for (int i = 0; i < objectList.Length; i++) {
				UIAtlas atlas = objectList [i].GetComponent<UIAtlas> ();
				if (atlas != null) {
					this.atlasList.Add (atlas.name, atlas);
//					Debug.Log (atlas.name);
				}
                
			}
		} else {
			this.isLoaded = false;
		}


	}

	/// <summary>
	/// 배경 불러오기
	/// </summary>
	/// <param name="fileName"></param>
	/// <returns></returns>
	//private IEnumerator LoadBackground(string fileName)
	//{
	//    //string[] prefabNames = { "Common_pack", "Common_pack_Atlas" };

	//    yield return StartCoroutine(dataManager.LoadGameObject(packageName, fileName, "Common_pack_Atlas"));
	//    if (dataManager.loadObj != null)
	//    {
	//        GameObject[] objectList = dataManager.loadObj;
	//        for (int i = 0; i < objectList.Length; i++)
	//        {
	//            this.commonAtlas = objectList[i].GetComponent<UIAtlas>();
	//        }
	//    }
	//    else
	//    {
	//        this.isLoaded = false;
	//    }
	//}


	/// <summary>
	/// 게임 효과 불러오기
	/// </summary>
	/// <param name="fileName"></param>
	/// <returns></returns>
	//private IEnumerator LoadEffect(string fileName)
	//{
	//    yield return StartCoroutine(dataManager.LoadGameObject(packageName, fileName, "Effect"));
	//    if (dataManager.loadObj != null)
	//    {
	//        GameObject[] objectList = dataManager.loadObj;
	//        for (int i = 0; i < objectList.Length; i++)
	//        {
	//            GameObject go = InstantiateGameObject(objectList[i]);
	//            this.effects = go.GetComponent<Effects>();
	//        }
	//    }
	//    else
	//    {
	//        this.isLoaded = false;
	//    }
	//}

	/// <summary>
	/// 게임내 팝업 불러오기
	/// </summary>
	/// <param name="fileName"></param>
	/// <returns></returns>
	//private IEnumerator LoadGamePopup(string fileName)
	//{
	//    yield return StartCoroutine(dataManager.LoadGameObject(packageName, fileName, "GamePopup"));
	//    if (dataManager.loadObj != null)
	//    {
	//        GameObject[] objectList = dataManager.loadObj;
	//        for (int i = 0; i < objectList.Length; i++)
	//        {
	//            GameObject go = InstantiateGameObject(objectList[i]);
	//            this.gamePopups = go.GetComponent<GamePopups>();
	//        }
	//    }
	//    else
	//    {
	//        this.isLoaded = false;
	//    }
	//}




	/// <summary>
	/// 메인 페이지 불러오기
	/// </summary>
	/// <param name="fileName"></param>
	/// <returns></returns>
	//private IEnumerator LoadPageMain(string fileName)
	//{
	//    string[] prefabNames = { "panel_main_down", "panel_main_up" };

	//    yield return StartCoroutine(dataManager.LoadGameObject(packageName, fileName, prefabNames));
	//    if (dataManager.loadObj != null)
	//    {
	//        GameObject[] objectList = dataManager.loadObj;
	//        if (objectList.Length == 2)
	//        {
	//            this.pageMainDown = objectList[0].GetComponent<WRPageMainDown>();
	//            this.pageMainUp = objectList[1].GetComponent<WRPageMainUp>();
	//        }
	//        else
	//        {
	//            Debug.LogError("LoagPageMain Error: " + fileName);
	//        }
	//    }
	//    else
	//    {
	//        this.isLoaded = false;
	//    }
	//}

	/// <summary>
	/// 질답 페이지 불러오기
	/// </summary>
	/// <param name="fileName"></param>
	/// <returns></returns>
	//private IEnumerator LoadPageQuestion(string fileName)
	//{
	//    yield return StartCoroutine(dataManager.LoadGameObject(packageName, fileName, new string[] { "Panel_page_Question_down", "Panel_page_Question_up" }));
	//    if (dataManager.loadObj != null)
	//    {
	//        GameObject[] objectList = dataManager.loadObj;
	//        if (objectList.Length == 2)
	//        {
	//            this.pageQDown = objectList[0].GetComponent<WRPageQuestionDown>();
	//            this.pageQUp = objectList[1].GetComponent<WRPageQuestionUp>();
	//        }
	//        else
	//        {
	//            Debug.LogError("LoagPageQuestion Error: " + fileName);
	//        }
	//    }
	//    else
	//    {
	//        this.isLoaded = false;
	//    }
	//}

	/// <summary>
	/// 시스템 팝업 불러오기
	/// </summary>
	/// <returns></returns>
	//public IEnumerator LoadPopup(string fileName)
	//{
	//    yield return StartCoroutine(dataManager.LoadGameObject(packageName, fileName, "Popup"));
	//    if (dataManager.loadObj != null)
	//    {
	//        GameObject[] objectList = dataManager.loadObj;
	//        for (int i = 0; i < objectList.Length; i++)
	//        {
	//            GameObject go = InstantiateGameObject(objectList[i]);
	//            this.popupWindows = go.GetComponent<PopupWindows>();
	//        }
	//    }
	//    else
	//    {
	//        this.isLoaded = false;
	//    }
	//}

	/// <summary>
	/// 튜토리얼 불러오기
	/// </summary>
	/// <returns></returns>
	//public IEnumerator LoadTutorial(string fileName)
	//{
	//    yield return StartCoroutine(dataManager.LoadGameObject(packageName, fileName, "Tutorial"));
	//    if (dataManager.loadObj != null)
	//    {
	//        GameObject[] objectList = dataManager.loadObj;
	//        for (int i = 0; i < objectList.Length; i++)
	//        {
	//            GameObject go = InstantiateGameObject(objectList[i]);
	//            this.tutorials = objectList[i].GetComponent<Tutorials>();
	//        }
	//    }
	//    else
	//    {
	//        this.isLoaded = false;
	//    }
	//}

	/// <summary>
	/// 게임 오브젝트 불러오기
	/// </summary>
	/// <returns></returns>
	//public IEnumerator LoadGameObject()
	//{
	//    string[] prefabNames = { PANEL_GAMEOBJECTS_NAME, PAGE_MAIN_DOWN_NAME, PAGE_MAIN_UP_NAME, PAGE_QUESTION_DOWN_NAME, PAGE_QUESTION_UP_NAME };

	//    yield return StartCoroutine(dataManager.LoadObject(packageName, GAMEOBJECT_NAME + EXT, prefabNames));
	//    // todo

	//    if (dataManager.loadObj != null)
	//    {
	//        GameObject[] objectList = dataManager.loadObj;
	//        for (int i = 0; i < objectList.Length; i++)
	//        {
	//            GameObject obj = objectList[i];
	//            if (obj.name == PANEL_GAMEOBJECTS_NAME)
	//            {
	//                GameObject go = GameObject.Instantiate(obj) as GameObject;
	//                go.transform.parent = uiroot.transform;
	//                go.transform.localPosition = Vector3.zero;
	//                go.transform.localScale = Vector3.one;

	//                gameObjects = go.GetComponent<GameObjects>();
	//            }
	//            else if (obj.name == (PAGE_MAIN_DOWN_NAME))
	//            {
	//                pageMainDown = obj.GetComponent<WRPageMainDown>();
	//            }
	//            else if (obj.name == (PAGE_MAIN_UP_NAME))
	//            {
	//                pageMainUp = obj.GetComponent<WRPageMainUp>();
	//            }
	//            else if (obj.name == (PAGE_QUESTION_DOWN_NAME))
	//            {
	//                pageQDown = obj.GetComponent<WRPageQuestionDown>();
	//            }
	//            else if (obj.name == (PAGE_QUESTION_UP_NAME))
	//            {
	//                pageQUp = obj.GetComponent<WRPageQuestionUp>();
	//            } 
	//        }

	//        SetLoadedBundle(GAMEOBJECT_NAME + EXT);
	//        isLoaded = true;
	//    }
	//    else
	//    {
	//        isLoaded = false;
	//    }
	//    dataManager.loadObj = null;
	//}

	/// <summary>
	/// 사운드 불러오기
	/// </summary>
	/// <returns></returns>
	public IEnumerator LoadSounds (string fileName)
	{
		yield return StartCoroutine (dataManager.LoadObject (packageName, fileName));
		if (dataManager.m_objectList != null) {
			SoundClips = new AudioClip[dataManager.m_objectList.Length];
			for (int i = 0; i < dataManager.m_objectList.Length; i++) {
				SoundClips [i] = (AudioClip)dataManager.m_objectList [i];
			}
		} else {
			isLoaded = false;
		}
		dataManager.m_objectList = null;
	}

	/// <summary>
	/// 사운드 불러오기
	/// </summary>
	/// <returns></returns>
	public IEnumerator LoadBGSounds (string fileName)
	{
		yield return StartCoroutine (dataManager.LoadObject (packageName, fileName));
		if (dataManager.m_objectList != null) {
			BgSoundClips = new AudioClip[dataManager.m_objectList.Length];
			for (int i = 0; i < dataManager.m_objectList.Length; i++) {
				BgSoundClips [i] = (AudioClip)dataManager.m_objectList [i];
			}
		} else {
			isLoaded = false;
		}
		dataManager.m_objectList = null;
	}

	/// <summary>
	/// 사진 번들 불러오기
	/// </summary>
	public IEnumerator LoadPictures (string fileName)
	{
		yield return StartCoroutine (dataManager.LoadObject (packageName, fileName));
		if (dataManager.m_objectList != null) {
			Pictures = new Texture2D[dataManager.m_objectList.Length];
			for (int i = 0; i < dataManager.m_objectList.Length; i++) {
				Pictures [i] = (Texture2D)dataManager.m_objectList [i];
			}
		} else {
			isLoaded = false;
		}
		dataManager.m_objectList = null;
	}
}

