using UnityEngine;
using System.Collections;
using System;


public enum StageLevel
{
    LEVEL_1 = 0,
    LEVEL_2,
    LEVEL_3,
    LEVEL_4,
    LEVEL_5,
    LEVEL_6,
    LEVEL_7,
    LEVEL_8,
    LEVEL_9,
    LEVEL_10
}

/// <summary>
/// 스테이지 형식으로 진행되는 게임에만 포함
/// </summary>
public class ZinStage 
{
    public static StageLevel ConvertStageLevel(string strStageLevel)
    {
        string[] stageLevels = Enum.GetNames(typeof(StageLevel));
        for (int i = 0; i < stageLevels.Length; i++)
        {
            if (stageLevels[i] == strStageLevel)
            {
                return (StageLevel)i;
            }
        }

        return StageLevel.LEVEL_1;
    }

    public static int ToInt(StageLevel stageLevel)
    {
        return (int)stageLevel;
    }

    public static string ToString(StageLevel stageLevel)
    {
        return ToInt(stageLevel).ToString();
    }

    public static StageLevel ConvertStageLevel(int stageLevel)
    {
        return (StageLevel)stageLevel;
    }
}
