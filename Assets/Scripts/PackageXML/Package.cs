using System.Collections;
using System.Xml.Serialization;

public class Package 
{
    public string ID;
    public string Name;
    public ZinBundle[] Bundles;
    //public string PreviewPicPath;
    public string PriceID;
    public string Price;
	public int Point;
	public string No;


    /// <summary>
    /// 구입여부
    /// </summary>
    public bool IsBuy;

    /// <summary>
    /// 다운로드 여부
    /// </summary>
    public bool IsDownload;

	public Package(){
	}
}
