using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    void Start()
    {
        string patha = Application.dataPath + "/" + "Test.xml";

        Package pack = new Package();
        pack.Name = "Test";

        ZinBundle[] bundles = new ZinBundle[2];
        bundles[0] = new ZinBundle(FileType.Picture, "Picture.unity3d");
        bundles[1] = new ZinBundle(FileType.PageMain, "PageMain.unity3d");
        pack.Bundles = bundles;
        pack.PriceID = "2,000";
        pack.ID = "dd";
        pack.IsBuy = true;
        pack.IsDownload = true;
        pack.Point = 11;
        pack.Price = "111";
		pack.No = "001";

        Package pack2 = new Package();
        pack2.Name = "Test2";
        pack.Bundles = bundles;
        pack2.PriceID = "2,000";
        pack.ID = "aa";
        pack.IsBuy = false;
        pack.IsDownload = false;
        pack.Point = 11;
        pack.Price = "1121";
		pack.No = "002";

        PackageList packList = new PackageList();
        packList.packages = new Package[] { pack, pack2 };


        if (ZinSerializerForXML.Serialization<PackageList>(packList, patha))
        {
            Debug.Log("save package List: " + patha);
        }
        else
        {
            Debug.LogError("Save Error: " + patha);
        }
	
	}
	
	void Update () {
	
	}
}
