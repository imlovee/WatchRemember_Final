using UnityEngine;
using System.Collections;

public class BuyPackage : ZinBehaviour
{
	public PopupWindows popupWindows;

	public string usd;
	public int cashPoint;

    /// <summary>
    /// 패키지 ID
    /// </summary>
    public string packId;

    /// <summary>
    /// 구입 ID
    /// </summary>
    public string buyId;

    void Start()
    {
		this.popupWindows = PopupWindows.Instance;


    }

    void GoogleIABManager_BillingResultOK(Item item, string id)
    {
        if (item == Item.PACK_ITEM && buyId == id)
        {
            Send(ButtonState.DOWNLOAD);
        }
    }



    public void Buy()
    {
		ZinWindow window = this.popupWindows.GetWindow ("SELECT_BUY");
		WindowSelectBuy selectWindow = window.GetComponent<WindowSelectBuy> ();
		selectWindow.Show (this.packId, this.buyId, this.usd, this.cashPoint, GetComponent<Purchaser>());
    }

}
