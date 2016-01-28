using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLevelList
{
    public List<GameLevel> gameLevels = new List<GameLevel>();

    private int levelCount;
    public int LevelCount
    {
        get { return gameLevels.Count; }
    }

    public void Add(GameLevel lv)
    {
        gameLevels.Add(lv);
    }

    public void Clear()
    {
        if (gameLevels != null)
        {
            gameLevels.Clear();
            gameLevels.TrimExcess();
        }
    }

    //public GameLevel GetLevel(int no)
    //{
    //    GameLevel result = gameLevels.Find(delegate(GameLevel lv)
    //    {
    //        return lv.No == no;
    //    });

    //    return result;
    //}


    public GameLevel GetLevel(int stageNo)
    {
        GameLevel result = gameLevels.Find(delegate(GameLevel lv)
        {
            int prevStageCount = (lv.No - 1) * lv.StageCount;
            int stageTotal = lv.No * lv.StageCount;

            return stageTotal > stageNo && prevStageCount <= stageNo;
        });

        return result;
    }
}
