using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class StageMgr : MonoBehaviour
{
    public Button BackBtn;
    public Button StageChangeBtn;

    [Header("Boss Stage")]
    public Button bossStageBtn;

    public GameObject[] StageList;
    public Image[] StageImg;
    private Outline StageOutline;

    public int stageCnt = 0;
    public int clearStageCnt = 0;
    public TextMeshProUGUI stageCntTxt;
    public TextMeshProUGUI chapterCntTxt;
    public static StageMgr Instance;

    [Header("Stage Buttons")]
    [SerializeField] private Transform stageButtonContainer;
    private List<Button> StageButtons = new List<Button>();

    private int StageCount = 0;

    GameObject player;

    // ─── 개발자 단축키: F1~F5 → 각 스테이지 보스씬 직행 ────────
    private static readonly string[] BossSceneNames =
    {
        "Stage1BossScene", // F1
        "Stage2BossScene", // F2
        "Stage3BossScene", // F3
        "Stage4BossScene", // F4
        "Stage5BossScene", // F5
    };

    private void Update()
    {
        for (int i = 0; i < BossSceneNames.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.F1 + i))
            {
                string sceneName = BossSceneNames[i];
                if (string.IsNullOrEmpty(sceneName))
                {
                    Debug.LogWarning($"[Dev] F{i + 1} 보스씬이 아직 등록되지 않았습니다.");
                    return;
                }
                Debug.Log($"[Dev] F{i + 1} → {sceneName}");
                SceneManager.LoadScene(sceneName);
                return;
            }
        }
    }

    void Start()
    {
        if (Instance == null)
            Instance = this;

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        ChapterManager.instance.Initialize();

        if (PlayerStateSaveManager.instance.IsLoadingFromSave)
        {
            RestoreFromSaveFile();
            PlayerStateSaveManager.instance.IsLoadingFromSave = false;
        }

        BuildStageButtons();
        OnViewStageIndex();
        SetStageScene();
        OnViewStageCnt();
    }

    private void BuildStageButtons()
    {
        StageButtons.Clear();
        ChapterData chapter = ChapterManager.instance.GetCurrentChapter();
        int count = chapter != null ? chapter.normalStageCount : 6;

        int i = 0;
        foreach (Transform child in stageButtonContainer)
        {
            Button btn = child.GetComponent<Button>();
            if (btn == null) continue;
            bool active = i < count;
            child.gameObject.SetActive(active);
            if (active) StageButtons.Add(btn);
            i++;
        }
    }

    private void RestoreFromSaveFile()
    {
        PlayerSaveData saveData = PlayerStateSaveManager.instance.Load();
        if (saveData == null)
        {
            Debug.LogWarning("[StageMgr] 세이브 파일 불러오기 실패 — 새 게임 상태 유지");
            return;
        }

        ChapterManager.instance.SetChapter(saveData.currentChapterId > 0 ? saveData.currentChapterId : 1);

        StageSaveManager.ResetStage();
        foreach (int stageId in saveData.clearedStageIds)
            StageSaveManager.ClearStage(stageId);

        PlayerStateSaveManager.instance.SetPendingRestore(saveData);
        Debug.Log($"[StageMgr] PlayerPrefs 재구성 완료, 복원 예약 (재개 스테이지: {saveData.resumeStageIndex}, 챕터: {saveData.currentChapterId})");
    }

    private void OnViewStageCnt()
    {
        stageCnt = StageButtons.Count - clearStageCnt;
        stageCntTxt.text = "Stage Cnt : " + stageCnt;
        if (chapterCntTxt != null)
            chapterCntTxt.text = "Chapter : " + ChapterManager.instance.CurrentChapterId;
    }

    public void OnResetStageInfo()
    {
        StageSaveManager.ResetStage();
        clearStageCnt = 0;
        OnViewStageIndex();
        SetStageScene();
        OnViewStageCnt();
    }

    private void OnViewStageIndex()
    {
        for (int i = 0; i < StageButtons.Count; i++)
        {
            StageButtons[i].GetComponent<NomalStage>().stageIdx = i;
        }
    }

    private void SetStageScene()
    {
        clearStageCnt = 0;

        foreach (var stage in StageButtons)
        {
            if (StageSaveManager.IsStageCleared(stage.GetComponent<NomalStage>().stageIdx))
            {
                stage.image.color = Color.blue;
                clearStageCnt++;
            }
            else
            {
                stage.image.color = Color.red;

                stage.onClick.RemoveAllListeners();
                stage.onClick.AddListener(() =>
                {
                    StageSaveManager.CurrentStageIdx = stage.GetComponent<NomalStage>().stageIdx;
                    OnInStageButton();
                });
            }
        }

        if (bossStageBtn == null)
        {
            Debug.LogError("[StageMgr] 보스 버튼이 할당되지 않았습니다.");
            return;
        }

        if (clearStageCnt >= StageButtons.Count)
        {
            bossStageBtn.gameObject.SetActive(true);
            bossStageBtn.interactable = true;

            bossStageBtn.onClick.RemoveAllListeners();
            bossStageBtn.onClick.AddListener(() => OnInBossStageButton());
        }
        else
        {
            bossStageBtn.interactable = false;
            bossStageBtn.image.color = Color.gray;
        }
    }

    private void StageChangeBtnClick()
    {
        StageCount++;
        player.transform.position = StageList[StageCount].transform.position;

        StageOutline.enabled = !StageOutline.enabled;
    }

    public void BackBtnClick()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnInStageButton()
    {
        SceneManager.LoadScene("IntegratedScene");
    }

    public void OnInBossStageButton()
    {
        ChapterData chapter = ChapterManager.instance.GetCurrentChapter();
        string bossScene = chapter != null ? chapter.bossSceneName : "Stage1BossScene";
        SceneManager.LoadScene(bossScene);
    }
}
