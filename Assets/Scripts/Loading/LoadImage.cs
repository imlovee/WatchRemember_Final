using UnityEngine;
using System.Collections;
using System.IO;

public class LoadImage : MonoBehaviour
{

	public string fileName;
	public UITexture mainImage;

	void Awake() {
		string filePath = string.Format ("{0}/{1}", Application.persistentDataPath, fileName);
		if (IsExist (filePath)) {
			StartCoroutine (Load ("file://" + filePath));
		} 

	}


	IEnumerator Start ()
	{
		string filePath = string.Format ("{0}/{1}", Application.persistentDataPath, fileName);
		string webPath = string.Format ("{0}/0B3VbOMZPLsy9bDZHZVhQaThjak0/{1}", PackageListManager.ServerPath, fileName);
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			yield return StartCoroutine (Load (webPath, filePath));
		}
	}

	private IEnumerator Load (string filePath)
	{
		using (WWW www = new WWW (filePath)) {
			yield return www;
			if (!www.isDone) {
				Debug.LogError (string.Format ("Load Image Error: LoadObject() :{0} WWWError", www.error));
			} else {
				if (www != null && www.error == null) {
					mainImage.mainTexture = www.texture;
					Debug.Log ("Load Image OK : " + mainImage.mainTexture);
				} else {
					Debug.LogError ("Load Error: " + filePath);
					Debug.LogError (www.error);
				}
			}
			www.Dispose ();
		}
	}

	private IEnumerator Load(string webPath, string filePath) {
		using (WWW www = new WWW (webPath)) {
			yield return www;
			if (!www.isDone) {
				Debug.LogError (string.Format ("Download Image Error: LoadObject() :{0} WWWError", www.error));
			} else {
				if (www != null && www.error == null) {
					File.WriteAllBytes (filePath, www.bytes);
					yield return StartCoroutine (Load ("file://" + filePath));
				} else {
					Debug.LogError ("Load Error: " + webPath);
					Debug.LogError (www.error);
				}
			}
			www.Dispose ();
		}
	}


	public bool IsExist (string filePath)
	{
		if (File.Exists (filePath)) {
			return true;
		} else {
			return false;
		}
	}
}
