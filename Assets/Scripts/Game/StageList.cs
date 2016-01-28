using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Mono.Data.SqliteClient;
using System;

public class StageList : MonoBehaviour
{
    public static StageList Instance;
//    public static StageList Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                GameObject go = new GameObject("StageManager");
//                instance = go.AddComponent<StageList>();
//                DontDestroyOnLoad(go);
//            }
//
//            return instance;
//        }
//    }

    private List<StageQuestion> m_list;
    public List<StageQuestion> List { get { return m_list; } }

    public int m_stageCount = -1;
    public readonly int ANSWER_TIME = 5;                    
    public readonly float DEFAULT_DELAY_TIME = 1f;                // 최초 이미지간 딜레이 
    public readonly float M_TIME = 0.025f;                            // 스테이지당 가중치
    public readonly int TOTAL_STAGE_COUNT = 20;

    public readonly int DEFAULT_IMG_COUNT = 3;                      // 최초 이미지 개수

    private GameLevelList levelList;

	public bool LastStage = false;

    void Awake()
    {
        Instance = this;
        m_list = new List<StageQuestion>();

        //m_db = GameObject.FindObjectOfType<DBManager>();
        //m_tableName = "StageS";
        //m_limitOffset = 0;
        //m_limitCount = 0;

    }

    void Start()
    {

        //LoadStage();
    }

    void OnDestroy()
    {
        Clear();
        m_list = null;
    }

    //public void LoadStage()
    //{
    //    Clear();

    //    SqliteDataReader reader = Select();
    //    Debug.Log(reader);

    //    if (reader != null)
    //    {
    //        while (reader.HasRows && reader.Read())
    //        {
    //            int stageid = reader.GetInt32(0);
    //            int stageNo = reader.GetInt32(1);
    //            int stageLevel = reader.GetInt32(2);

    //            StageQuestion stage = (StageQuestion)ZinSerializer.Deserialization<StageQuestion>(reader.GetString(3));
    //            stage.Id = stageid;
    //            stage.Stage_no = stageNo;
    //            stage.Stage_level = (StageLevel)stageLevel;
    //            if (stage.Answers == null || stage.Answers.Length == 0)
    //            {
    //                Debug.LogError("stage error(answer not found):" + stageid);
    //            }
    //            //stage.Print();

    //            m_list.Add(stage);
    //        }
    //    }

    //    Send();
    //}

    /// <summary>
    /// 스테이지 별로 보여지는 이미지간의 딜레이 타임 설정
    /// </summary>
    /// <returns></returns>
    private float[] GetViewTimes()
    {
        float[] viewTimes = new float[TOTAL_STAGE_COUNT];
        for (int i = 0; i < viewTimes.Length; i++)
        {
            viewTimes[i] = DEFAULT_DELAY_TIME - (i * M_TIME);
        }

        return viewTimes;
    }

    /// <summary>
    /// 스테이지 별로 보여지는 이미지 장수 설정
    /// </summary>
    /// <returns></returns>
    private int[] GetImageCount()
    {
        int[] imgCounts = new int[TOTAL_STAGE_COUNT];
        int addImgCount = -1;
        for (int i = 0; i < imgCounts.Length; i++)
        {
            if (i % 2 == 0)
            {
                addImgCount++;
            }
            imgCounts[i] = DEFAULT_IMG_COUNT + addImgCount;
        }

        return imgCounts;
    }

    public void SetStageList(string xml)
    {
        levelList = ZinSerializerForXML.Deserialization<GameLevelList>(xml);
        //m_stageCount = levelList.LevelCount;

        //Debug.Log(m_stageCount);

        //GameLevelList list = new GameLevelList();

        //GameLevel lv1 = new GameLevel();
        //lv1.Init();
        //list.Add(lv1);

        //GameLevel lv2 = new GameLevel();
        //lv2.Init();
        //list.Add(lv2);

        //ZinSerializerForXML.Serialization<GameLevelList>(list, Application.dataPath + "/GameLevelList.xml");
    }


    //public void LoadStage()
    //{
    //    float[] viewTimes = GetViewTimes();
    //    int[] imgCounts = GetImageCount();

    //    Array arr = Enum.GetNames(typeof(StageLevel));

    //    m_stageCount = arr.Length;
    //    for (int i = 0; i < m_stageCount; i++)
    //    {
    //        StageQuestion stage = new StageQuestion();
    //        stage.Stage_level = (StageLevel)(i % (TOTAL_STAGE_COUNT / 2) + 1);
    //        stage.Delay_time = viewTimes[i] / imgCounts[i];
    //        stage.Answer_time = ANSWER_TIME;
    //        stage.Image_count = imgCounts[i];

    //        m_list.Add(stage);
    //    }
    //}

    private void Clear()
    {
        if (m_list != null)
        {
            m_list.Clear();
            m_list.TrimExcess();
        }
    }

    //public StageQuestion GetStage(int level)
    //{
    //    if (m_list.Count == 0)
    //    {
    //        return null;
    //    }

    //    StageQuestion stage = m_list[level];

    //    Debug.Log("StageNo : " + stage.Stage_level);
    //    Debug.Log("Remain Count :" + m_list.Count);

    //    return stage;
    //}

    public GameLevel GetLevel(int stageNo)
    {
        if (levelList == null) return null;

        Debug.Log("Stage No : " + stageNo);
		this.LastStage = IsLastStage (stageNo);

        return levelList.GetLevel(stageNo);
    }

	public bool IsLastStage(int stageNo) {
		if (levelList.GetLevel (stageNo + 1) == null)
			return true;
		
		return false;
	}
}
