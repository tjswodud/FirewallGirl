using UnityEngine;

/// <summary>
/// SPE_005 "강제 재부팅" 기능 테스트용 스크립트.
/// BattleScene Play 모드에서 임의의 GameObject에 붙인 뒤,
/// 인스펙터 컨텍스트 메뉴(우클릭) → "Run Force Reboot Test"로 실행한다.
/// </summary>
public class ForceRebootCardObjectTest : MonoBehaviour
{
    [ContextMenu("Run Force Reboot Test")]
    private void RunTest()
    {
        if (PlayerManager.instance == null)
        {
            Debug.LogError("[테스트] PlayerManager.instance가 없습니다. BattleScene Play 모드에서 실행하세요.");
            return;
        }

        // 1. 플레이어에게 버프 1개, 디버프 1개 부여
        PlayerManager.instance.AddMultiTurnStat(StatType.Attack, 2, 3, "테스트 버프");
        PlayerManager.instance.cannotGainDefenseTurns = 2;
        PlayerManager.instance.UpdateUI();

        bool beforeOk = PlayerManager.instance.activeModifiers.Count == 1
            && PlayerManager.instance.cannotGainDefenseTurns == 2;
        Debug.Log(beforeOk
            ? "[테스트] 사전 조건 통과: 버프 1개 / 디버프 1개 적용됨"
            : "[테스트 실패] 사전 조건 실패: 버프/디버프가 정상적으로 적용되지 않음");

        // 2. SPE_005 카드 효과 실행
        ForceRebootCardObject card = ScriptableObject.CreateInstance<ForceRebootCardObject>();
        card.ApplyEffect(PlayerManager.instance);
        PlayerManager.instance.UpdateUI();

        // 3. 검증
        bool afterOk = PlayerManager.instance.activeModifiers.Count == 0
            && PlayerManager.instance.cannotGainDefenseTurns == 0;

        Debug.Log(afterOk
            ? "[테스트 성공] SPE_005 사용 후 모든 버프/디버프가 제거되었습니다."
            : "[테스트 실패] SPE_005 사용 후에도 버프/디버프가 남아있습니다.");
    }
}
