using UnityEngine;

public static class StageSaveManager
{
    public static int CurrentStageIdx = 0;

    private static int GetTotalNormalStages()
    {
        ChapterData chapter = ChapterManager.instance.GetCurrentChapter();
        return chapter != null ? chapter.normalStageCount : 6;
    }

    // ─── 스테이지 클리어 상태 ──────────────────────────────────────────────────

    /// <summary>해당 스테이지를 클리어 처리한다.</summary>
    public static void ClearStage(int stageID)
    {
        if (PlayerPrefs.GetInt($"Stage_{stageID}", 0) == 0)
        {
            PlayerPrefs.SetInt($"Stage_{stageID}", 1);
            PlayerPrefs.Save();
            Debug.Log($"스테이지 {stageID} 클리어 저장 완료!");
        }
    }

    /// <summary>현재 챕터의 모든 일반 스테이지 클리어 데이터를 초기화한다.</summary>
    public static void ResetStage()
    {
        int total = GetTotalNormalStages();
        for (int i = 0; i < total; i++)
        {
            if (IsStageCleared(i))
                PlayerPrefs.SetInt($"Stage_{i}", 0);
        }
        PlayerPrefs.Save();
        Debug.Log("스테이지 클리어 초기화 완료!");
    }

    /// <summary>특정 스테이지의 클리어 여부를 반환한다.</summary>
    public static bool IsStageCleared(int stageID)
    {
        return PlayerPrefs.GetInt($"Stage_{stageID}", 0) == 1;
    }

    /// <summary>모든 일반 스테이지가 클리어되었는지 확인한다.</summary>
    public static bool CanEnterBossStage()
    {
        int total = GetTotalNormalStages();
        for (int i = 0; i < total; i++)
        {
            if (!IsStageCleared(i))
                return false;
        }
        return true;
    }

    // ─── 챕터 보스 스테이지 클리어 상태 ──────────────────────────────────────

    /// <summary>해당 챕터의 보스 스테이지를 클리어 처리한다.</summary>
    public static void ClearBossStage(int chapterId)
    {
        PlayerPrefs.SetInt($"Chapter_{chapterId}_Boss", 1);
        PlayerPrefs.Save();
        Debug.Log($"챕터 {chapterId} 보스 클리어 저장 완료!");
    }

    /// <summary>해당 챕터의 보스 스테이지 클리어 여부를 반환한다.</summary>
    public static bool IsBossStageCleared(int chapterId)
    {
        return PlayerPrefs.GetInt($"Chapter_{chapterId}_Boss", 0) == 1;
    }

    /// <summary>해당 챕터의 보스 스테이지 클리어 데이터를 초기화한다.</summary>
    public static void ResetBossStage(int chapterId)
    {
        PlayerPrefs.SetInt($"Chapter_{chapterId}_Boss", 0);
        PlayerPrefs.Save();
    }
}
