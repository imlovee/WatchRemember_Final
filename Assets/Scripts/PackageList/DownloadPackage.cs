using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;

public class DownloadPackage : ZinBehaviour
{
	private PackageListManager packageList;
	public string packageId;
	public UIProgressBar bar;

	public float progressValue = 0f;

	/// <summary>
	/// 다운로드중 
	/// </summary>
	public bool isDownloading = false;


	/// <summary>
	/// 다운로드 성공여부 
	/// </summary>
	public bool isLoaded = false;



	void Start ()
	{
		packageList = PackageListManager.Instance;
	}


	private string GetPath ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			return "Android";
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return "IOS";
		} else if (Application.platform == RuntimePlatform.OSXPlayer) {
			return "MAC"; 
		} else {
			return "MAC";
		}
	}


	private IEnumerator LoadBundle (string fileName)
	{
		string path = string.Format ("{0}/{1}/{2}/{3}/{4}", PackageListManager.ServerPath, packageList.FolderID, packageId, GetPath (), fileName);
		string savePath = string.Format ("{0}/{1}/{2}/{3}", Application.persistentDataPath, packageList.PackagesFolder, packageId, fileName);

		//using (WWW www = new WWW(path))
		WWW www = new WWW (path);
		{
			while (!www.isDone) {
				//progressValue = www.progress;
				//bar.value = progressValue;
				//Debug.Log(string.Format("Progress - {0}%. from {1}", progressValue, www.url));
				yield return null;
			}

			//yield return www;

			if (www.isDone && www.error == null) {
				//yield return new WaitForSeconds(0.2f);
				if (www.bytes != null) {
#if !UNITY_WEBPLAYER
					try {
						File.WriteAllBytes (savePath, www.bytes);
					} catch (System.Exception ex) {
						Debug.LogError ("Download Failed : " + ex.ToString ());

						isLoaded = false;
					} finally {
						Debug.Log ("Download Complete: " + savePath);
					}
#endif
				}
			} else {
				Debug.LogError ("Download Package error: " + path);
			}
			www.Dispose ();
		}
	}

	public static bool WebExists (string url)
	{

		bool ret = true;
		if (url == null)
			return false;

		HttpWebResponse response = null;

		try {
			var request = (HttpWebRequest)WebRequest.Create (url);
			request.Method = "HEAD";
			response = (HttpWebResponse)request.GetResponse ();
		} catch (WebException) {
			ret = false;
		} finally {
			if (response != null) {
				response.Close ();
			}
		}

		return ret;
	}

	IEnumerator Download ()
	{
		isDownloading = true;
		Package package = packageList.packList.GetPackage (packageId);
		Debug.Log ("Downloading Package: " + packageId);

		//string[] bundleNames = package.BundleNames;
		ZinBundle[] bundles = package.Bundles;
		bar.value = 0;

		this.isLoaded = true;
		for (int i = 0; i < bundles.Length; i++) {
			yield return StartCoroutine (LoadBundle (bundles [i].BundleName));

			switch (bundles [i].BundleType) {
			case FileType.BGSound:
				break;
			case FileType.Common_pack:
				yield return StartCoroutine (LoadBundle (bundles [i].GetBundleXML ()));
				break;
			case FileType.Effect:
				yield return StartCoroutine (LoadBundle (bundles [i].GetBundleXML ()));
				break;
			case FileType.GameLevel:
				break;
			case FileType.GamePopup:
				yield return StartCoroutine (LoadBundle (bundles [i].GetBundleXML ()));
				break;
			case FileType.Language:
				break;
			case FileType.PageMain:
				yield return StartCoroutine (LoadBundle (bundles [i].GetBundleXML ()));
				break;
			case FileType.PageQuestion:
				yield return StartCoroutine (LoadBundle (bundles [i].GetBundleXML ()));
				break;
			case FileType.Picture:
				break;
			case FileType.Popup:
				yield return StartCoroutine (LoadBundle (bundles [i].GetBundleXML ()));
				break;
			case FileType.Sound:
				break;
			case FileType.Tutorial:
				yield return StartCoroutine (LoadBundle (bundles [i].GetBundleXML ()));

				break;
			default:
				break;
			}

			bar.value = (float)(i + 1) / bundles.Length;
			yield return new WaitForSeconds (0.1f);

			if (!isLoaded)
				break;
		}
		isDownloading = false;

		if (isLoaded) {

			PlayerPrefs.SetInt (NameManager.PREF_PACKAGE_DOWNLOAD_HEADER + packageId, 1);
			PlayerPrefs.Save ();

			Send (ButtonState.PLAY);
		} else {
			//Send (ButtonState.DOWNLOAD);
			bar.value = 1f;


			//todo : show error window 

		}
	}




	public void StartDownload ()
	{
		if (!isDownloading) {
			StartCoroutine (Download ());
		}
	}
}
