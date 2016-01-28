using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PackageListManager : MonoBehaviour
{
	private static PackageListManager instance;

	public static PackageListManager Instance {
		get {
			if (instance == null) {
				GameObject go = new GameObject ("PackageListManager");
				instance = go.AddComponent<PackageListManager> ();
			}
			return PackageListManager.instance;
		}
	}


	public static string ServerPath = "https://googledrive.com/host";
	public string FolderID = "0B3VbOMZPLsy9bS14TDZkQl9TYzA";
	public string FileName = "PackageList.xml";
	public string PackagesFolder = "Package";

	public string WathRememberPath{
		get {
			return string.Format("{0}/{1}", PackageListManager.ServerPath, FolderID);
		}
	}

	private string xml;
	public PackageList packList;
	public Texture[] previewImages;

	/// <summary>
	/// xml 로드 완료시점
	/// </summary>
	/// <param name="loaded">로드 성공/실패</param>
    public delegate void OnLoadXml (bool loaded);

	public event OnLoadXml onLoadXml;

	public delegate void ListLoaded (bool loaded);

	public event ListLoaded onListLoaded;

	private string packagesPath;

	private int fileCount = 0;

	public int FileCount {
		get {
			if (packList != null) {
				fileCount = packList.packages.Length;
			}
			return fileCount;
		}
	}


	void Start ()
	{
		PackageManager.Instance.onLoaded += PackageManager_Instance_onLoaded;
		string path = string.Format ("{0}/{1}/{2}", ServerPath, FolderID, FileName);

		StartCoroutine (LoadXML (path));
	}

	void Awake ()
	{
		

		//test
		//string patha = Application.dataPath + "/" + FileName;

		//Package pack = new Package();
		//pack.Name = "Test";

		//ZinBundle[] bundles = new ZinBundle[2];
		//bundles[0] = new ZinBundle(FileType.PICTURE, "Picture.unity3d");
		//bundles[1] = new ZinBundle(FileType.PAGE_MAIN, "PageMain.unity3d");
		//pack.Bundles = bundles;
		//pack.PriceID = "2,000";
		//pack.ID = "dd";
		//pack.IsBuy = true;
		//pack.IsDownload = true;
		//pack.Point = "CC";
		//pack.Price = "111";

		//Package pack2 = new Package();
		//pack2.Name = "Test2";
		//pack.Bundles = bundles;
		//pack2.PriceID = "2,000";
		//pack.ID = "aa";
		//pack.IsBuy = false;
		//pack.IsDownload = false;
		//pack.Point = "AAA";
		//pack.Price = "1121";

		//PackageList packList = new PackageList();
		//packList.packages = new Package[] { pack, pack2 };

		//Debug.Log("save package List: " +  patha);

		//ZinSerializerForXML.Serialization<PackageList>(packList, patha);
	}

	void PackageManager_Instance_onLoaded (bool loaded)
	{
		if (loaded) {
			StartCoroutine (LoadPreviewImage ());
			SetStatus ();
		}
	}

	public void SetLoadXml (bool loaded)
	{
		if (onLoadXml != null) {
			onLoadXml (loaded);
		}
	}

	/// <summary>
	/// 패키지를 저장할 폴더 확인
	/// </summary>
	private void CheckFolder ()
	{
		packagesPath = string.Format ("{0}/{1}", Application.persistentDataPath, PackagesFolder);
		CreateFolder (packagesPath);

		for (int i = 0; i < packList.packages.Length; i++) {
			string packageFolder = string.Format ("{0}/{1}", packagesPath, packList.packages [i].ID);
			CreateFolder (packageFolder);
		}
	}


	/// <summary>
	/// 폴더 생성(존재하지 않을시)
	/// </summary>
	/// <param name="path"></param>
	private void CreateFolder (string path)
	{
		if (!Directory.Exists (path)) {
			Directory.CreateDirectory (path);
		}
	}




	private void SetListLoaded (bool loaded)
	{
		if (onListLoaded != null) {
			onListLoaded (loaded);
		}
	}

	public string GetPath (string path)
	{
		if (Application.platform == RuntimePlatform.Android) {
			path = string.Format ("file:{0}", path);
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			path = string.Format ("file://{0}", path);
		} else {
			path = string.Format ("file://{0}", path);
		}
		return path;
	}

	/// <summary>
	/// 프리뷰 이미지 불러오기
	/// </summary>
	/// <returns></returns>
	IEnumerator LoadPreviewImage ()
	{
		CheckFolder ();

		if (packList != null) {
			Package[] packages = packList.packages;
			previewImages = new Texture[packages.Length];


			string previewFileName = "Preview.png";
			bool loadComplete = true;
			bool isLocal = false;

			string path = string.Empty;
			for (int i = 0; i < packages.Length; i++) {
				string filePath = string.Format ("{0}/{1}/{2}", packagesPath, packages [i].ID, previewFileName);
				if (File.Exists (filePath)) {
					path = GetPath (packagesPath);
					isLocal = true;

				} else {
					// 파일이 로컬에 없을시에 웹에서 다운받도록 경로 수정
					path = string.Format ("{0}/{1}", ServerPath, FolderID);
					isLocal = false;
				}

				string imgPath = string.Format ("{0}/{1}/{2}", path, packages [i].ID, previewFileName);
				Debug.Log ("ImagePath: " + imgPath);
				using (WWW www = new WWW (imgPath)) {
					yield return www;
					if (www.isDone && string.IsNullOrEmpty (www.error)) {
						if (!isLocal) {
							// 로컬에 저장
							File.WriteAllBytes (filePath, www.bytes);
						}
						previewImages [i] = www.texture;
						previewImages [i].name = packages [i].ID;
					} else {
						Debug.LogError (string.Format ("Preview Images Loading Error: {0}, {1}", packages [i].ID, www.error));
						loadComplete = false;
					}

					www.Dispose ();
				}
			}

			if (loadComplete) {
				Debug.Log ("Package List Load Complete");
			}
			SetListLoaded (loadComplete);
		}
	}


	/// <summary>
	/// 패키지 상태 설정(구입여부/다운로드여부)
	/// </summary>
	private void SetStatus ()
	{
		Package[] packs = packList.packages;
		string prefix = "pack_";
		for (int i = 0; i < packs.Length; i++) {
			Package pack = packs [i];

			if (!pack.IsBuy && !pack.IsDownload) {
				bool isVaild = true;

				for (int k = 0; k < pack.Bundles.Length; k++) {
					if (!GameDataManager.Instance.IsExist (pack.ID, pack.Bundles [k].BundleName)) {
						isVaild = false;
					}
				}
				pack.IsDownload = isVaild;

				#if UNITY_ANDROID
                //todo : 구매여부 확인
                pack.IsBuy = GoogleIABManager.Instance.IsBuy(prefix + pack.ID);
				#endif
			}
		}
	}



	/// <summary>
	/// xml 불러오기
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	IEnumerator LoadXML (string path)
	{
		using (WWW www = new WWW (path)) {
			yield return www;
			if (www.isDone) {
				xml = www.text;
				packList = (PackageList)ZinSerializerForXML.Deserialization<PackageList> (xml);
				if (packList != null) {
					Debug.Log ("LoadXML Compleate: " + path);
					SetLoadXml (true);
				} else {
					Debug.LogError ("LoadXML Error: " + path);
					SetLoadXml (false);
				}


//                StartCoroutine(LoadPreviewImage());
//                SetStatus();
			} else {
				Debug.LogError ("XML Not Found:" + path);
				Debug.LogError (www.error);
			}

			www.Dispose ();
		}
	}

}
