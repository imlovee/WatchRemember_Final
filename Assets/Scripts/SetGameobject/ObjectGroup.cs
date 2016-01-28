using UnityEngine;
using System.Collections;

public class ObjectGroup : MonoBehaviour
{
    PackageManager pack;
    public FileType type;


    public bool SetWidget(PackageManager package)
    {
        this.pack = package;
        return SetSprite() && SetLabel() ? true : false;
    }


    /// <summary>
    /// Sprite 설정
    /// </summary>
    /// <returns></returns>
    bool SetSprite()
    {
        if (!this.pack.atlasXml.ContainsKey(gameObject.name))
        {
            Debug.LogError("해당 이름을 찾을수 없습니다. : " + gameObject.name);
            return false;
        }

        XMLObjectGroup xmlGrp = this.pack.atlasXml[gameObject.name];
        XMLSprite[] spriteXml = xmlGrp.Sprites;
        UISprite[] sprites = GetComponentsInChildren<UISprite>();

		string atlasName = string.Empty;
        for (int i = 0; i < spriteXml.Length; i++)
        {
			UIAtlas atlas = null;
			atlasName = spriteXml [i].AtlasName;
			if (this.pack.atlasList.ContainsKey (atlasName)) {
				atlas = this.pack.atlasList [atlasName];

			} else if (atlasName.Equals ("Common_Atlas")) {
				atlas = Common.Instance.CommonAtlas;
			} else {
				Debug.LogError("Atlas Not Found: " + spriteXml [i].AtlasName);
			}

			if (sprites [i].name == spriteXml [i].Name) {
				sprites [i].atlas = atlas;		
				sprites [i].spriteName = spriteXml [i].SpriteName;
				spriteXml [i].GetValue (sprites [i]);
			}
        }

        return true;
    }

    /// <summary>
    /// Label 설정
    /// </summary>
    /// <returns></returns>
    bool SetLabel()
    {
        if (!this.pack.atlasXml.ContainsKey(gameObject.name))
        {
            Debug.LogError("해당 이름을 찾을수 없습니다. : " + gameObject.name);

            return false;
        }

        
        XMLObjectGroup xmlGrp = this.pack.atlasXml[gameObject.name];
        XMLLabel[] labelXml = xmlGrp.Labels;
        UILabel[] label = GetComponentsInChildren<UILabel>();

        for (int i = 0; i < labelXml.Length; i++)
        {
            UIFont font = this.pack.fontList[labelXml[i].FontName];
            labelXml[i].GetValue(label[i]);
            label[i].bitmapFont = font;
        }



        return true;
    }

}
