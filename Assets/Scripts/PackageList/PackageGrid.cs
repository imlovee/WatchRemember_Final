using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackageGrid : MonoBehaviour
{
	public GameObject packageItem;
	public GameObject currentItem;
	public ControlAd controlAd;
	public UICenterOnChild onChild;
	public WindowMessageOK WindowBuyOK;
	public ZinWindow WindowBuyFail;

	private readonly string BUNDLE_NAME = "com.NewDayX.LookNRemember";
	PackageListManager packManager;

	public SortedList<string, GameObject> itemList = new SortedList<string, GameObject> ();

	void Start ()
	{
		this.onChild = GetComponent<UICenterOnChild> ();

		packManager = PackageListManager.Instance;
		packManager.onListLoaded += Instance_onListLoaded;
	}

	void Instance_onListLoaded (bool loaded)
	{
		InitItem ();
		Debug.Log ("Load PackageList OK"); 
	}

	void OnDestroy ()
	{
		if (this.itemList != null) {
			this.itemList.Clear ();
			this.itemList.TrimExcess ();
		}
	}


	void InitItem ()
	{
		PackageList packs = packManager.packList;


		string textureName = "Texture";
		string label_title_name = "Label_title";
		string label_price_name = "Label_price";
		string setbtnFnName = "SetButtons";


		this.itemList.Clear ();

		for (int i = 0; i < packManager.previewImages.Length; i++) {
			GameObject obj = null;

			obj = Instantiate (packageItem) as GameObject;

			UITexture texture = ZinUtil.GetChildObj (obj, textureName).GetComponent<UITexture> ();
			texture.material = Instantiate (texture.material) as Material;
			texture.mainTexture = packManager.previewImages [i];


			if (packs.packages [i].ID == PackageManager.Instance.packageName) {

				obj.GetComponent<SetDummyPackageItem> ().SetItem ();
				packs.packages [i].IsBuy = true;

			} else if (packs.packages [i].ID == NameManager.PREF_PLAY_PACKAGE_DEFAULT) {
				packs.packages [i].IsBuy = true;
				packs.packages [i].IsDownload = true;
			}
			else {
				packs.packages [i].IsBuy = PlayerPrefs.GetInt (NameManager.PREF_PACKAGE_BUY_HEADER + packs.packages [i].PriceID, 0) == 1 ? true : false;
			}

			ChangeButton changeBtn = obj.GetComponent<ChangeButton> ();
			SetBtnState (ref packs.packages [i], ref changeBtn, ref texture);

			UILabel titleLabel = ZinUtil.GetChildObj (obj, label_title_name).GetComponent<UILabel> ();
			UILabel priceLabel = ZinUtil.GetChildObj (obj, label_price_name).GetComponent<UILabel> ();

			if (changeBtn.state == ButtonState.PLAY) {
				priceLabel.text = string.Empty;
			} else {
				priceLabel.text = GetPriceText (ref packs.packages [i]);
			}


			//				if (!packs.packages[i].IsDownload)
			{
				DownloadPackage dp = obj.GetComponent<DownloadPackage> ();
				dp.packageId = packs.packages [i].ID;
				dp.SetNotify (obj, setbtnFnName);
			}

			PlayPackage pk = obj.GetComponent<PlayPackage> ();
			pk.PackageId = packManager.packList.packages [i].ID;

			if (!packs.packages [i].IsBuy ) 
			{
				BuyPackage buy = obj.GetComponent<BuyPackage> ();
				buy.packId = packs.packages [i].ID;
				buy.SetNotify (obj, setbtnFnName);
				buy.buyId = packs.packages [i].PriceID;
				buy.usd = packs.packages [i].Price;

				buy.cashPoint = packs.packages [i].Point;

				Purchaser purch = obj.AddComponent<Purchaser> ();
				purch.kProductNameAppleNonConsumable = packs.packages [i].PriceID;
				purch.BuyOK = WindowBuyOK;
				purch.BuyFail = WindowBuyFail;
			}

			titleLabel.text = packManager.packList.packages [i].Name;

			itemList.Add (packs.packages [i].No, obj);
		}

		// 정렬된 오브젝트 배치
		foreach (GameObject item in itemList.Values) {

			if (item == currentItem) {
			} else {
				item.transform.parent = this.transform;
				item.transform.localScale = Vector3.one;
				item.transform.localPosition = Vector3.zero;
			}
		}
//		controlAd.SetScreen ();

		GetComponent<UIGrid> ().Reposition ();
		onChild.CenterOn (currentItem.transform);


	}

	/// <summary>
	/// 기기에 저장된 구입 여부 
	/// </summary>
	/// <returns><c>true</c> if this instance is buy the specified packageId; otherwise, <c>false</c>.</returns>
	/// <param name="packageId">Package identifier.</param>
	private bool IsBuy (string packageId)
	{
		int val = PlayerPrefs.GetInt (NameManager.PREF_PACKAGE_BUY_HEADER + packageId, 0);

		return val == 0 ? false : true;
	}

	private bool IsDownload (string packageId)
	{
		int val = PlayerPrefs.GetInt (NameManager.PREF_PACKAGE_DOWNLOAD_HEADER + packageId, 0); 

		return val == 0 ? false : true;
	}


	public GameObject GetItem (int index)
	{
		if (this.itemList == null)
			return null;

		foreach (GameObject item in this.itemList.Values) {
			ItemClick itemClick = item.GetComponent<ItemClick> ();
			if (itemClick.index == index) {
				return item;
			}
		}
		return null;
	}

	/// <summary>
	/// 구입 완료 시 
	/// </summary>
	/// <param name="index">Index.</param>
	public void BuyOK ()
	{

		GameObject go = GetItem (ItemClick.SelectIndex);
		ChangeButton cb = go.GetComponent<ChangeButton> ();
		cb.SetButtons (ButtonState.DOWNLOAD);
	}

	/// <summary>
	/// 가격 표시 텍스트 가져오기
	/// </summary>
	/// <param name="pack"></param>
	/// <returns></returns>
	private string GetPriceText (ref Package pack)
	{
		if (!string.IsNullOrEmpty (pack.Price) && pack.Point >= 0) {
			return string.Format ("{0}$ / {1}", pack.Price, pack.Point);
		} else if (!string.IsNullOrEmpty (pack.Price)) {
			return string.Format ("{0}$", pack.Price);
		} else if (pack.Point >= 0) {
			return string.Format ("{0}", pack.Point);
		}

		return string.Empty;
	}


	private void SetBtnState (ref Package pack, ref ChangeButton changeBtn, ref UITexture texture)
	{
		if (pack.ID == PlayerPrefs.GetString (NameManager.PREF_PLAY_PACKAGE, NameManager.PREF_PLAY_PACKAGE_DEFAULT)) {
			changeBtn.SetButtons (ButtonState.NONE);
			texture.material.SetFloat ("_EffectAmount", 1);

			changeBtn.SetButtons (ButtonState.NONE);
			return;
		} else {
			texture.material.SetFloat ("_EffectAmount", 0);
		}

		bool isBuy = IsBuy (pack.ID);
		bool isDownlaod = IsDownload (pack.ID);

		if ((pack.IsBuy && pack.IsDownload) || (isBuy && isDownlaod)) {
			changeBtn.SetButtons (ButtonState.PLAY);
		} else if (pack.IsBuy || isBuy) {
			changeBtn.SetButtons (ButtonState.DOWNLOAD);
		} else {
			changeBtn.SetButtons (ButtonState.BUY);
		}
	}


}
