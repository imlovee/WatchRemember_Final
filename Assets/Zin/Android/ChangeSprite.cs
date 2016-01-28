using UnityEngine;
using System.Collections;


public enum LanguageImage {
	ENG = 0,
	KOR
}

/// <summary>
/// 언어에 따라 이미지 변경
/// </summary>
public class ChangeSprite : MonoBehaviour
{
	public UISprite sprite;
	public string[] spriteNames;
	public LanguageImage currentLang = LanguageImage.ENG;


	void Start ()
	{
		string spriteName = string.Empty;

		switch (Application.systemLanguage) {
		case SystemLanguage.Korean:
			spriteName = spriteNames [(int)LanguageImage.KOR];
			this.currentLang = LanguageImage.KOR;
			break;
		default:
			spriteName = spriteNames [(int)LanguageImage.ENG];
			this.currentLang = LanguageImage.ENG;
			break;
		} 

		this.sprite.spriteName = spriteName;
		this.sprite.MakePixelPerfect ();

	}
	
}
