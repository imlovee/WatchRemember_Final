using UnityEngine;
using System.Collections;

public class GameLevel
{
    public string Name;
    public int No;
    public int StageCount;
    public float DelayTime;
    public int CountDown;
    public int ImageCount;
    public int ClearScore;
    public int ClearCashPoint;

    public GameLevel()
    {

    }

    public void Init()
    {
        Name = "레벨 이름";
        No = 0;
        StageCount = 1;
        DelayTime = 1f;
        CountDown = 5;
        ImageCount = 3;
        ClearScore = 100;
        ClearCashPoint = 10;
    }

}
