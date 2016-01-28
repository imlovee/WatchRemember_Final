using UnityEngine;
using System.Collections;

public class PopupListToDBWhere : ZinBehaviours
{
    public string m_whereColumnName;
    public string m_operator;

    void Start()
    {

    }

    public void SetDBWhere(GameObject go)
    {
        if (string.IsNullOrEmpty(m_whereColumnName) || string.IsNullOrEmpty(m_operator)) return;

        UIPopupList popupList = go.GetComponent<UIPopupList>();
        if (popupList == null) return;

        int index = popupList.items.FindIndex(delegate(string str)
        {
            return str.Equals(popupList.value);
        });

        //string where = string.Format("{0} {1} '{2}'", m_whereColumnName, m_operator, index);
        DBWhere where = new DBWhere(m_whereColumnName, m_operator, index.ToString());

        Send(where);
    }

    public void SendDBWhere()
    {
        SetDBWhere(gameObject);
    }
}
