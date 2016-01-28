using UnityEngine;
using System.Collections;


/// <summary>
/// 로드된 게임 오브젝트 배치
/// </summary>
public class SetGameobjects : MonoBehaviour
{
    private SetGameobjects instance;
    public SetGameobjects Instance
    {
        get {
            if (instance == null)
            {
                GameObject go = new GameObject("PackGameObjects");
                instance = go.AddComponent<SetGameobjects>();
            }
            return instance; 
        }
    }

    public PackageManager pack;


    void Awake()
    {
        this.pack = PackageManager.Instance;
        this.pack.onLoaded += pack_onLoaded;
    }

    /// <summary>
    /// 패키지 로드 완료시
    /// </summary>
    /// <param name="loaded"></param>
    void pack_onLoaded(bool loaded)
    {
        if (loaded)
        {
            
        }
        else
        {
            Debug.LogError("SetGameobject Error");
        }
    }

    void Start()
    {

    }

}
