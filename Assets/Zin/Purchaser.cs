using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, one of the existing Survival Shooter scripts.

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class Purchaser : MonoBehaviour, IStoreListener
{
	private IStoreController m_StoreController;
	// Reference to the Purchasing system.
	private IExtensionProvider m_StoreExtensionProvider;
	// Reference to store-specific Purchasing subsystems.

	// Product identifiers for all products capable of being purchased: "convenience" general identifiers for use with Purchasing, and their store-specific identifier counterparts
	// for use with and outside of Unity Purchasing. Define store-specific identifiers also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

	public string kProductIDNonConsumable = "nonconsumable";
	public string kProductNameAppleNonConsumable = "NOADS";
	public string kProductNameGooglePlayNonConsumable = "NOADS";

	public WindowMessageOK BuyOK;
	public ZinWindow BuyFail;

	public static bool isRun = false;

	void Start ()
	{

	}

	public void InitializePurchasing ()
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized ()) {
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());

		builder.AddProduct (kProductIDNonConsumable, ProductType.NonConsumable, new IDs () { {
				kProductNameAppleNonConsumable,
				AppleAppStore.Name
			}, {
				kProductNameGooglePlayNonConsumable,
				GooglePlay.Name
			},
		});// And finish adding the subscription product.
		UnityPurchasing.Initialize (this, builder);

	}


	private bool IsInitialized ()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}


	public void BuyNonConsumable ()
	{
		if (Purchaser.isRun)
			return;
		
		if (m_StoreController == null) {
			Purchaser.isRun = true;
			InitializePurchasing ();
		} 
	}

	void BuyProductID (string productId)
	{
		// If the stores throw an unexpected exception, use try..catch to protect my logic here.
		try {
			// If Purchasing has been initialized ...
			if (IsInitialized ()) {
				// ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
				Product product = m_StoreController.products.WithID (productId);

				// If the look up found a product for this device's store and that product is ready to be sold ... 
				if (product != null && product.availableToPurchase) {
					Debug.Log (string.Format ("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
					m_StoreController.InitiatePurchase (product);
				}
					// Otherwise ...
					else {
					// ... report the product look-up failure situation  
					Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
					Debug.Log (productId);
				}
			}
				// Otherwise ...
				else {
				// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
				Debug.Log ("BuyProductID FAIL. Not initialized.");
			}
		}
			// Complete the unexpected exception handling ...
			catch (Exception e) {
			// ... by reporting any unexpected exception for later diagnosis.
			Debug.Log ("BuyProductID: FAIL. Exception during purchase. " + e);
		}
	}


	// Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
	public void RestorePurchases ()
	{
		// If Purchasing has not yet been set up ...
		if (!IsInitialized ()) {
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log ("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer ||
		    Application.platform == RuntimePlatform.OSXPlayer) {
			// ... begin restoring purchases
			Debug.Log ("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions> ();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions ((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
				Debug.Log ("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				if (!string.IsNullOrEmpty (kProductNameAppleNonConsumable)) {
					PlayerPrefs.SetInt (NameManager.PREF_PACKAGE_BUY_HEADER + kProductNameAppleNonConsumable, 1);
					PlayerPrefs.Save ();
				}
			});
		}
			// Otherwise ...
			else {
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log ("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}


	//
	// --- IStoreListener
	//

	public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log ("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;

		BuyProductID (kProductIDNonConsumable);
	}


	public void OnInitializeFailed (InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log ("OnInitializeFailed InitializationFailureReason:" + error);

		Purchaser.isRun = false;
	}


	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args)
	{
		if (String.Equals (args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal)) {
			Debug.Log (string.Format ("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

			BuyComplete (kProductNameAppleNonConsumable);


		}
		// Or ... an unknown product has been purchased by this user. Fill in additional products here.
		else {
			Debug.Log (string.Format ("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));

			BuyFail.Show ();
		}// Return a flag indicating wither this product has completely been received, or if the application needs to be reminded of this purchase at next app launch. Is useful when saving purchased products to the cloud, and when that save is delayed.
		return PurchaseProcessingResult.Complete;
	}

	public void BuyComplete (string id)
	{
		Purchaser.isRun = false;
		PlayerPrefs.SetInt (NameManager.PREF_PACKAGE_BUY_HEADER + id, 1);
		PlayerPrefs.Save ();

		if (kProductNameAppleNonConsumable == NameManager.PREF_BUY_NOADS) {
			Application.LoadLevel (1);
		} else {
			BuyOK.Show (id);
		}
	}


	public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason)
	{
		Purchaser.isRun = false;
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
		Debug.Log (string.Format ("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}
}
