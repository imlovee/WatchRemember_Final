using UnityEngine;
using System.Collections;

public class SetTutorialLanguage : MonoBehaviour
{
    public SetTutorialWindow tuto_1;
    public SetTutorialWindow tuto_2;
    public SetTutorialWindow tuto_3;
    public SetTutorialWindow tuto_4;

    private LanguageManager lang;
    private PackageLanguage packLang;

    void Awake()
    {
        lang = LanguageManager.Instance;
    }

    void Start()
    {
        PackageManager.Instance.onLoaded += Instance_loadLanguage;
    }

    void Instance_loadLanguage(bool loaded)
    {
        if (loaded)
        {
            packLang = lang.pack;

            tuto_1.SetText(0, packLang.GetText("Tutorial_Step_1_Music/Sound"));
            tuto_1.SetText(1, packLang.GetText("Tutorial_Step_1_Record"));
            tuto_1.SetText(2, packLang.GetText("Tutorial_Step_1_Ad"));
            tuto_1.SetText(3, packLang.GetText("Tutorial_Step_1_Setting"));
            tuto_1.SetText(4, packLang.GetText("Tutorial_Step_1_Gamestart"));

            tuto_2.SetText(0, packLang.GetText("Tutorial_Step_2"));

            tuto_3.SetText(0, packLang.GetText("Tutorial_Step_3"));

            tuto_4.SetText(0, packLang.GetText("Tutorial_Step_4"));
        }
    }


}

