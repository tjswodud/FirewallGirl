using UnityEngine;

[CreateAssetMenu(menuName = "Game/Chapter Data")]
public class ChapterData : ScriptableObject
{
    public int chapterId;
    public int normalStageCount;
    public string bossSceneName;
    public bool isFinalChapter;
}
