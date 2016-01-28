using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook.Unity;
using Facebook.Unity.Canvas;
using Facebook.Unity.Editor;
using Facebook.Unity.Mobile;
using Facebook.Unity.Mobile.Android;
using Facebook.Unity.Mobile.IOS;
using Facebook.MiniJSON;

public class FBManager : MonoBehaviour
{
	public static FBManager Instance;

	public Texture2D lastResponseTexture { get; set; }

	string LastResponse;


	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		if (FBManager.Instance == null) {
			Instance = this;
		}

		Init ();
	}

	public void Init ()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init (InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp ();
		}
	}




	#region FB.Init

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp ();
			// Continue with Facebook SDK
			// ...
			Debug.Log ("ActivateApp OK");
		} else {
			Debug.Log ("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	#endregion

	#region FB.Login()

	public void CallFBLogin ()
	{
		var perms = new List<string> (){ "public_profile", "email", "user_friends" };
		FB.LogInWithReadPermissions (perms, LoginCallback);
	}

	void LoginCallback (IResult result)
	{
		if (FB.IsLoggedIn) {
			var aToken = AccessToken.CurrentAccessToken;
			Debug.Log (aToken.UserId);

			foreach (string perm in aToken.Permissions) {
				Debug.Log (perm);
			}
		} else {
			Debug.Log ("User cancelled login");
		}

	}

	private void CallFBLogout ()
	{
		FB.LogOut ();
	}

	#endregion

	#region FB.AppRequest() Friend Selector

	/// <summary>
	/// 친구 리스트 열기(리스트에서 메세지 보낼 상대 선택)
	/// </summary>
	/// <param name="title">메세지 제목</param>
	/// <param name="message">메세지</param>
	/// <param name="filter">FriendSelectorFilters = "[\"app_users\"]";</param>
	/// <param name="data">"{}"</param>
	/// <param name="excludeIds"></param>
	/// <param name="max"></param>
	//    private void CallAppRequestAsFriendSelector(string title, string message, string filters, string data, string excIds, string max)
	//    {
	//        // If there's a Max Recipients specified, include it
	//        int? maxRecipients = null;
	//        if (max != "")
	//        {
	//            try
	//            {
	//                maxRecipients = Int32.Parse(max);
	//            }
	//            catch (Exception e)
	//            {
	//                Debug.LogError(e.Message);
	//            }
	//        }
	//
	//        // include the exclude ids
	//        string[] excludeIds = (excIds == "") ? null : excIds.Split(',');
	//        List<object> FriendSelectorFiltersArr = null;
	//        if (!string.IsNullOrEmpty(filters))
	//        {
	//            try
	//            {
	//                FriendSelectorFiltersArr = Facebook.MiniJSON.Json.Deserialize(filters) as List<object>;
	//            }
	//            catch
	//            {
	//                throw new Exception("JSON Parse error");
	//            }
	//        }
	//
	//        FB.AppRequest(
	//            message,
	//            null,
	//            FriendSelectorFiltersArr,
	//            excludeIds,
	//            maxRecipients,
	//            data,
	//            title,
	//            Callback
	//        );
	//    }
	#endregion

    #region FB.AppRequest() Direct Request

    //public string DirectRequestTitle = "";
    //public string DirectRequestMessage = "Herp";
    //private string DirectRequestTo = "";

    /// <summary>
    /// 메세지 보내기
    /// </summary>
    /// <param name="title">메세지 제목</param>
    /// <param name="message">메세지 내용</param>
    /// <param name="toID">보낼 상대 ID(콤마로 구분)</param>
//    private void CallAppRequestAsDirectRequest(string title, string message, string toID)
//    {
//        if (title == "")
//        {
//            throw new ArgumentException("\"To Comma Ids\" must be specificed", "to");
//        }
//        FB.AppRequest(
//            message,
//            toID.Split(','),
//            null,
//            null,
//            null,
//            "",
//            title,
//            Callback
//        );
//    }

    #endregion

    #region FB.Feed() 

    //public string FeedToId = "";
    //public string FeedLink = "";
    //public string FeedLinkName = "";
    //public string FeedLinkCaption = "";
    //public string FeedLinkDescription = "";
    //public string FeedPicture = "";
    //public string FeedMediaSource = "";
    //public string FeedActionName = "";
    //public string FeedActionLink = "";
    //public string FeedReference = "";
    public bool IncludeFeedProperties = false;
	private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]> ();

	//    public void CallFBFeed(string feedToId, string feedLink, string linkName, string linkCaption, string linkDesc, string pic, string media, string actionName, string actionLink, string reference)
	//    {
	//        Dictionary<string, string[]> feedProperties = null;
	//        if (IncludeFeedProperties)
	//        {
	//            feedProperties = FeedProperties;
	//        }
	//        FB.Feed(
	//            toId: feedToId,
	//            link: feedLink,
	//            linkName: linkName,
	//            linkCaption: linkCaption,
	//            linkDescription: linkDesc,
	//            picture: pic,
	//            mediaSource: media,
	//            actionName: actionName,
	//            actionLink: actionLink,
	//            reference: reference,
	//            properties: feedProperties,
	//            callback: Callback
	//        );
	//    }

	#endregion

	//
	private void Callback (IResult result)
	{
		if (result == null) {
			this.LastResponse = "Null Response\n";
			Debug.LogError (this.LastResponse);
			return;
		}

		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty (result.Error)) {
			this.LastResponse = "Error Response:\n" + result.Error;
			Debug.LogError (result.Error);
		} else if (result.Cancelled) {
			this.LastResponse = "Cancelled Response:\n" + result.RawResult;
			Debug.Log (this.LastResponse);
		} else if (!string.IsNullOrEmpty (result.RawResult)) {
			this.LastResponse = "Success Response:\n" + result.RawResult;
			Debug.Log (this.LastResponse);
		} else {
			this.LastResponse = "Empty Response\n";
			Debug.LogError (this.LastResponse);
		}
	}

	/// <summary>
	/// 공유 기능
	/// </summary>
	public void Share ()
	{
		#if UNITY_ANDROID
		string url = "https://play.google.com/store/apps/details?id=com.NewDayX.LookNRemember";
		#elif UNITY_IOS
		string url = "https://play.google.com/store/apps/details?id=com.NewDayX.LookNRemember";
		#else 
		string url = "https://play.google.com/store/apps/details?id=com.NewDayX.LookNRemember";
		#endif

		FB.ShareLink (
			new Uri (url),
			"Watch Remember",
			"Watch Remember is very fun!",
			new Uri ("http://www.newdayx.com/images/watch_remember_web.jpg"),
			callback: this.Callback);
	}

	private void CallFeed ()
	{
		if (FB.IsLoggedIn) {
			string imgUrl = "";
//            CallFBFeed("", "", "linkname", "linkcaption", "linkdesc", imgUrl, "", "actionName", "actionLink", "reference");
		}
	}

	private IEnumerator TakeScreenshot ()
	{
		yield return new WaitForEndOfFrame ();

		var width = Screen.width;
		var height = Screen.height;
		var tex = new Texture2D (width, height, TextureFormat.RGB24, false);
		// Read screen contents into the texture
		tex.ReadPixels (new Rect (0, 0, width, height), 0, 0);
		tex.Apply ();
		byte[] screenshot = tex.EncodeToPNG ();

		var wwwForm = new WWWForm ();
		wwwForm.AddBinaryData ("image", screenshot, "Screenshot.png");

		FB.API ("me/photos", HttpMethod.POST, APICallback, wwwForm);
	}

	private void APICallback (IGraphResult result)
	{
		if (result == null) {
			this.LastResponse = "Null API Response\n";
			Debug.LogError (this.LastResponse);
			return;
		}

		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty (result.Error)) {
			this.LastResponse = "Error API Response:\n" + result.Error;
			Debug.LogError (result.Error);
		} else if (result.Cancelled) {
			this.LastResponse = "Cancelled API Response:\n" + result.RawResult;
			Debug.Log (this.LastResponse);
		} else if (!string.IsNullOrEmpty (result.RawResult)) {
			this.LastResponse = "Success API Response:\n" + result.RawResult;
			Debug.Log (this.LastResponse);

			var dict = Json.Deserialize (result.RawResult) as Dictionary<string,object>;
			string id = dict ["id"] as string;

			FB.ShareLink (
				new Uri ("https://play.google.com/store/apps/details?id=com.NewDayX.LookNRemember"),
				"Watch Remember",
				"Watch Remember is very fun!",
				new Uri ("http://www.facebook.com/" + id),
				callback: this.Callback);
		} else {
			this.LastResponse = "Empty API Response\n";
			Debug.LogError (this.LastResponse);
		}
	}

}
