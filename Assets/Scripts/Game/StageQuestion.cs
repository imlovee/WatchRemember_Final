using UnityEngine;
using System.Collections;


/// <summary>
/// Stage는 게임마다 저장 형식이 다를수 있으므로 이클래스를 생성해야함
/// </summary>
/// 
public class StageQuestion 
{
    private StageLevel m_stage_level = StageLevel.LEVEL_1;
    public StageLevel Stage_level
    {
        get { return m_stage_level; }
        set { m_stage_level = value; }
    }

    private float m_delay_time = -1;
    public float Delay_time
    {
        get { return m_delay_time; }
        set { m_delay_time = value; }
    }

    private int m_answer_time = -1;
    public int Answer_time
    {
        get { return m_answer_time; }
        set { m_answer_time = value; }
    }

    private int m_image_count = -1;
    public int Image_count
    {
        get { return m_image_count; }
        set { m_image_count = value; }
    }


    public StageQuestion() { }


}
