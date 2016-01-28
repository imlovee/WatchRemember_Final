using UnityEngine;
using System.Collections;
using System.IO;

public class GameDataManager : MonoBehaviour
{
	private static GameDataManager instance;

	public static GameDataManager Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject ("GameDataManager");
				instance = go.AddComponent<GameDataManager> ();
			}
			return instance;
		}
	}

	public object[] m_objectList;
	public GameObject[] loadObj;
	public string xml;
	public string serverURL = "https://drive.google.com/host/";

	void Start ()
	{
	}

	/// <summary>
	/// 패키지 경로 가져오기
	/// </summary>
	/// <param name="packageName"></param>
	/// <param name="fileName"></param>
	/// <returns></returns>
	//private string GetPersistentPath(string packageName, string fileName)
	//{
	//    string path = string.Format("{0}/Packague/{1}/{2}", Application.persistentDataPath, packageName, fileName);
	//    if (!File.Exists(path))
	//    {
	//        path = "https://drive.google.com/host/0B3VbOMZPLsy9NFFIMnJnc2RHX1E/Picture.unity3d"

	//    } else {
	//        path = "file://" + path;
	//    }

	//    return path;
	//}


	/// <summary>
	/// 파일 존재 유무 확인
	/// </summary>
	/// <param name="packageName"></param>
	/// <param name="fileName"></param>
	/// <returns></returns>
	public bool IsExist (string packageName, string fileName)
	{
		string path = GetPackagePath (packageName, fileName);
		if (File.Exists (path)) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// unity3d 경로 가져오기
	/// </summary>
	/// <param name="packageName"></param>
	/// <param name="fileName"></param>
	/// <returns></returns>
	private string GetPackagePath (string packageName, string fileName)
	{
		return string.Format ("{0}/{1}/{2}/{3}", Application.persistentDataPath, PackageListManager.Instance.PackagesFolder, packageName, fileName);
	}

	/// <summary>
	/// 기본 경로 가져오기
	/// </summary>
	/// <param name="packageName">패키지 이름</param>
	/// <param name="fileName">파일 이름</param>
	/// <returns></returns>
	public string GetPath (string packageName, string fileName)
	{
		string path = GetPackagePath (packageName, fileName);
		if (File.Exists (path)) {
			if (Application.platform == RuntimePlatform.Android) {
				path = string.Format ("file:/{0}", path);
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				path = string.Format ("file:{0}", path);

			} else {
				path = string.Format ("file:{0}", path);
			}

			return path;

		}

		// 해당 파일이 없을 경우 기본패키지에서 불러옴
		string defaultPackageName = NameManager.PREF_PLAY_PACKAGE_DEFAULT;

		if (Application.platform == RuntimePlatform.WindowsEditor) {
			path = string.Format ("file:/{0}/Editor/Bundle/{1}/{2}/{3}", Application.dataPath, defaultPackageName, "Window", fileName);
		} else if (Application.platform == RuntimePlatform.OSXEditor) {
			path = string.Format ("file://{0}/Editor/Bundle/{1}/{2}/{3}", Application.dataPath, defaultPackageName, "Mac", fileName);
		}
		else if (Application.platform == RuntimePlatform.Android) {
			path = string.Format ("jar:file://{0}!/assets/{1}/{2}/{3}", Application.dataPath, defaultPackageName, "Android", fileName);
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			path = string.Format ("file://{0}/{1}/{2}/{3}", Application.streamingAssetsPath, defaultPackageName, "IOS", fileName);
		}

		return path;
	}

	public IEnumerator LoadObject (string packageName, string fileName)
	{
		yield return StartCoroutine (LoadObject (GetPath (packageName, fileName)));
	}

	public IEnumerator LoadGameObject (string packageName, string fileName, string[] prefabName)
	{
		yield return StartCoroutine (LoadGameObject (GetPath (packageName, fileName), prefabName));
	}

	public IEnumerator LoadGameObject (string packageName, string fileName, string prefabName)
	{
		string[] prefabs = new string[1];
		prefabs [0] = prefabName;

		yield return StartCoroutine (LoadGameObject (GetPath (packageName, fileName), prefabs));
	}

	private IEnumerator LoadGameObject (string path, string[] prefabName)
	{
		using (WWW www = new WWW(path)) {
			yield return www;
			if (!www.isDone || www.error != null) {
				Debug.LogError (string.Format ("ResourcesLoadManager: LoadObject() :{0} WWWError", www.error));
			} else {
				if (www != null && www.error == null) {
					m_objectList = www.assetBundle.LoadAllAssets<GameObject> ();
					loadObj = new GameObject[m_objectList.Length];
					for (int i = 0; i < loadObj.Length; i++) {
						loadObj[i] = m_objectList [i] as GameObject;
					}
					Debug.Log ("Load Complete: " + path);

				} else {
					Debug.LogError ("Load Error: " + path);
				}
			}
			www.assetBundle.Unload (false);
			www.Dispose ();
		}
	}

	public IEnumerator LoadObject (string path)
	{
		using (WWW www = new WWW(path)) {
			yield return www;
			if (!www.isDone) {
				Debug.LogError (string.Format ("ResourcesLoadManager: LoadObject() :{0} WWWError", www.error));
			} else {
				if (www != null && www.error == null) {
					m_objectList = www.assetBundle.LoadAllAssets ();
					Debug.Log ("Load Complete: " + path + " - " + m_objectList.Length);
				} else {
					Debug.LogError ("Load Error: " + path);
					Debug.LogError (www.error);
				}

			}

			www.assetBundle.Unload (false);
			www.Dispose ();
		}
	}

	public IEnumerator LoadXML (string packageName, string fileName)
	{
		yield return StartCoroutine (LoadXML (GetPath (packageName, fileName)));
	}

	public IEnumerator LoadXML (string path)
	{
		//Debug.Log("Start Load: " + path);
		using (WWW www = new WWW(path)) {
			yield return www;
			if (!www.isDone || www.error != null) {
				Debug.LogError (string.Format ("ResourcesLoadManager: LoadXML() :{0} WWWError", www.error));
				Debug.LogError (path);
			} else {
				xml = www.text;
				Debug.Log ("Load Complete: " + path);
			}

			www.Dispose ();
		}

	}

	public IEnumerator LoadObject (string path, string[] names)
	{
		using (WWW www = new WWW(path)) {
			//www.progress

			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				Debug.LogError (string.Format ("ResourcesLoadManager: LoadObject() :{0} WWWError", www.error));
				Debug.LogError (path);
			} else {
				if (www != null && www.assetBundle != null) {
					Debug.Log ("Load Complete: " + path);
				}

			}

			www.assetBundle.Unload (false);
			www.Dispose ();
		}
	}
}
