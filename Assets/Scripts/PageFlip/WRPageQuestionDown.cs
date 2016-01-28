using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WRPageQuestionDown : WRPage
{
    // default
    public UIPanel m_defaultUI;

    public Texture2D[] m_Answer_images;
    public UITexture[] m_Answer_backgrounds;
    public InitToggle[] m_toggles;
    public UIButton[] m_buttons;
	public GameObject m_clickBackGound;


    public void SetAnswerImages(List<Texture2D> textures)
    {
        if (textures == null) return;

        m_Answer_images = new Texture2D[textures.Count];
        for (int i = 0; i < textures.Count; i++)
        {
            m_Answer_images[i] = textures[i];
        }
    }

    /// <summary>
    /// 답변 이미지 위치 가져오기
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector3 GetAnswerPosition(int index)
    {
        if (index < 0 || index >= m_Answer_backgrounds.Length) return Vector3.zero;

        return m_Answer_backgrounds[index].transform.localPosition;
    }


    /// <summary>
    /// 선택한 값 가져오기
    /// </summary>
    public int GetSelectAnswer()
    {
        for (int i = 0; i < m_toggles.Length; i++)
        {
            if (m_toggles[i].GetComponent<UIToggle>().value)
            {
                return i;
            }
        }
        return -1;
    }


    // step1

    public void SetDefaultUI(bool isShow)
    {
        if (m_defaultUI == null) return;

        m_defaultUI.gameObject.SetActive(isShow);

        if (isShow)
        {
            SetToggles(0);
        }

        m_clickBackGound.gameObject.SetActive(!isShow);
    }

    /// <summary>
    /// 보기 버튼 설정(이벤트)
    /// </summary>
    public void SetButtons(MonoBehaviour beghviour, string functionName)
    {
        for (int i = 0; i < m_buttons.Length; i++)
        {
            m_buttons[i].onClick.Add(new EventDelegate(beghviour, functionName));
            m_buttons[i].onClick.Add(new EventDelegate(this, "ToggleClick"));
        }
    }

    private void ToggleClick()
    {
		if (!TutorialManager.Instance.isPlaying) {
			m_clickBackGound.gameObject.SetActive (true);
		}
    }

    private void SetToggles(int selectIndex)
    {
        for (int i = 0; i < m_toggles.Length; i++)
        {
            m_toggles[i].SetToggle(selectIndex);
        }
    }

    public void SetQuestionUI(bool isShow)
    {
        SetToggles(1);
        SetAnswers();
    }

    private void SetAnswers()
    {
        for (int i = 0; i < m_Answer_backgrounds.Length; i++)
        {
            m_Answer_backgrounds[i].mainTexture = m_Answer_images[i];
        }
    }


}
