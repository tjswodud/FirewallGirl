using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 챕터 진행 상태를 관리하는 순수 C# 싱글톤.
/// MonoBehaviour가 아니므로 씬에 배치할 필요 없이 첫 접근 시 자동 생성된다.
/// </summary>
public class ChapterManager
{
    private static ChapterManager _instance;
    public static ChapterManager instance => _instance ??= new ChapterManager();

    private ChapterData[] _allChapters;

    public int CurrentChapterId { get; private set; } = 1;

    /// <summary>챕터 보스 클리어 후 DeckBuildingScene 진입 시 현재 덱을 사전 선택하도록 DeckManager에 알림.</summary>
    public bool IsChapterTransition { get; set; } = false;

    /// <summary>Resources/Chapters/ 에서 ChapterData를 로드한다. StageMgr.Start()에서 호출.</summary>
    public void Initialize()
    {
        if (_allChapters != null && _allChapters.Length > 0) return;

        _allChapters = Resources.LoadAll<ChapterData>("Chapters");
        System.Array.Sort(_allChapters, (a, b) => a.chapterId.CompareTo(b.chapterId));
        Debug.Log($"[ChapterManager] 챕터 데이터 {_allChapters.Length}개 로드 완료");
    }

    public ChapterData GetCurrentChapter()
    {
        if (_allChapters == null || _allChapters.Length == 0)
        {
            Debug.LogWarning("[ChapterManager] 챕터 데이터가 로드되지 않았습니다. Initialize()를 먼저 호출하세요.");
            return null;
        }

        foreach (ChapterData chapter in _allChapters)
        {
            if (chapter.chapterId == CurrentChapterId)
                return chapter;
        }

        Debug.LogWarning($"[ChapterManager] chapterId={CurrentChapterId}에 해당하는 ChapterData를 찾지 못했습니다.");
        return _allChapters[0];
    }

    /// <summary>세이브 복원 시 챕터를 지정한다.</summary>
    public void SetChapter(int id)
    {
        CurrentChapterId = id;
        Debug.Log($"[ChapterManager] 챕터 {id}로 복원");
    }

    /// <summary>다음 챕터로 이동하며 스테이지 클리어 데이터를 초기화한다.</summary>
    public void AdvanceChapter()
    {
        CurrentChapterId++;
        StageSaveManager.ResetStage();
        IsChapterTransition = true;
        Debug.Log($"[ChapterManager] 챕터 {CurrentChapterId}로 진행");
    }

    /// <summary>새 게임 시작 시 챕터를 1로 초기화한다.</summary>
    public void ResetToChapter1()
    {
        CurrentChapterId = 1;
        IsChapterTransition = false;
        Debug.Log("[ChapterManager] 새 게임: 챕터 1로 초기화");
    }

    /// <summary>보스 클리어 후 최종 챕터이면 게임 클리어, 아니면 다음 챕터 덱 편집으로 이동한다.</summary>
    public void HandleBossClear()
    {
        ChapterData current = GetCurrentChapter();
        if (current != null && current.isFinalChapter)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.GameClear();
            else
                Debug.LogError("[ChapterManager] GameManager.Instance가 null입니다.");
        }
        else
        {
            AdvanceChapter();
            SceneManager.LoadScene("DeckBuildingScene");
        }
    }
}
