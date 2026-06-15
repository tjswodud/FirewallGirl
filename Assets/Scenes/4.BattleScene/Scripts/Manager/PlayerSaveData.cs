using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSaveData
{
    public int currentHP;
    public int fullHP;
    public int[] baseStats;
    public List<string> augmentIds = new List<string>();
    public List<CardSaveData> deck = new List<CardSaveData>();
    public List<int> clearedStageIds = new List<int>();
    public int resumeStageIndex;
    public int currentChapterId = 1;
}

[Serializable]
public class CardSaveData
{
    public int cardIndex;
    public int positiveStatValue;
    public int negativeStatValue;
    public int cost;
    public int coolTime;
}
