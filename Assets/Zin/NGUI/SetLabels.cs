using UnityEngine;
using System.Collections;

public class SetLabels : MonoBehaviour
{
    public GameObject m_labelPrefab;
    public int m_count = 0;
    public string[] m_texts;
    public int m_cellWidth = 100;
    public int m_cellHeight = 30;
    public int m_xCount = 0;

    public bool isStart = false;

    private UILabel[] m_labels;
    public UILabel[] Labels { get { return m_labels; } }

    void Start()
    {
        if (isStart)
        {
            SetLabelAll();
        }
    }

    public void InitText()
    {
        ClearLabels();

        m_texts = new string[m_count];
        for (int i = 0; i < m_count; i++)
        {
            m_texts[i] = string.Empty;
        }
    }

    //public void SetLabelColor(int index, Color color)
    //{
    //    if (index >= m_labels.Length)
    //    {
    //        Debug.LogError("out of index");
    //        return;
    //    }

    //    m_labels[index].color = color;
    //}

    public void ClearLabels()
    {
        ZinTools.DestroyGameObjects<UILabel>(m_labels);
    }

    public void SetLabelAll(string[] texts, int xCount)
    {
        m_xCount = xCount;

        SetLabelAll(texts);
    }


    public void SetLabelAll(string[] texts)
    {
        m_texts = texts;
        m_count = texts.Length;

        ClearLabels();
        SetLabelAll();
    }

    void SetLabelAll()
    {
        if (m_texts == null || m_texts.Length != m_count) return;

        m_labels = new UILabel[m_texts.Length];
        for (int i = 0; i < m_count; i++)
        {
            GameObject labelGo = GameObject.Instantiate(m_labelPrefab) as GameObject;
            labelGo.transform.parent = transform;
            labelGo.transform.localScale = Vector3.one;
            labelGo.transform.localPosition = Vector2.zero;

            UILabel label = labelGo.GetComponent<UILabel>();
            label.text = m_texts[i];

            m_labels[i] = label;
        }

        UIGrid grid = gameObject.GetComponent<UIGrid>();
        if (grid == null)
        {
            grid = gameObject.AddComponent<UIGrid>();
        }
        grid.cellWidth = m_cellWidth;
        grid.cellHeight = m_cellHeight;
        grid.maxPerLine = m_xCount;
        grid.enabled = true;
    }
}
