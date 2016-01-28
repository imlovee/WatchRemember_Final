using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestionImages : MonoBehaviour
{
    public List<Texture2D> m_textures;
    public Texture2D m_EndTexture;

    public List<Texture2D> m_QImagesList;

    void Awake()
    {
        m_textures = new List<Texture2D>();
        m_QImagesList = new List<Texture2D>();

        PackageManager.Instance.onLoaded += Instance_onLoaded;
    }

    private void Instance_onLoaded(bool loaded)
    {
        if (loaded)
        {
            m_textures.AddRange(PackageManager.Instance.Pictures);
        }
    }

    //IEnumerator Start()
    //{
    //    yield return StartCoroutine(LoadImages());
    //    Debug.Log("Load Images Complete");
    //}
    void Start()
    {

    }

    void OnDestroy()
    {
        if (m_textures != null)
        {
            m_textures.Clear();
            m_textures.TrimExcess();
        }
        m_textures = null;

        ClearQList();
        m_QImagesList = null;
    }

    public void ClearQList()
    {
        if (m_QImagesList != null)
        {
            m_QImagesList.Clear();
            m_QImagesList.TrimExcess();
        }
    }

    //IEnumerator LoadImages()
    //{
    //    Texture2D[] objs = Resources.LoadAll<Texture2D>("GameImages/QuestionImagesAll");

    //    for (int i = 0; i < objs.Length; i++)
    //    {
    //        m_textures.Add((Texture2D)objs[i]);
    //        yield return null;
    //    }
    //}

    public Texture2D GetRandomImage(ref List<Texture2D> textureList)
    {
        if (m_textures == null || m_textures.Count == 0) return null;

        Texture2D texture = m_textures[Random.Range(0, m_textures.Count)];
        if (textureList.Contains(texture))
        {
            return GetRandomImage(ref textureList);
        }
        else
        {
            return texture;
        }

    }

    public List<Texture2D> GetExtraImages(int count)
    {
        int length = m_textures.Count < count ? m_textures.Count : count;

        List<Texture2D> useTextures = new List<Texture2D>();
        for (int i = 0; i < length; i++)
        {
            Texture2D texture = GetRandomImage(ref useTextures);
            useTextures.Add(texture);
            m_QImagesList.Add(texture);
        }

        return m_QImagesList;
    }

    public List<Texture2D> GetExtraImages(int count, ref List<Texture2D> useTextures)
    {
        int length = m_textures.Count < count ? m_textures.Count : count;

        //List<Texture2D> useTextures = used;
        for (int i = 0; i < length; i++)
        {
            Texture2D texture = GetRandomImage(ref useTextures);
            useTextures.Add(texture);
            m_QImagesList.Add(texture);
        }

        return m_QImagesList;
    }

}
