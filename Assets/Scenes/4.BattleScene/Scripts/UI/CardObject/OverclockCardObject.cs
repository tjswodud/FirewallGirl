using UnityEngine;

/// <summary>
/// SPE_001 오버클럭(OVERCLOCK): 이번 턴 코스트 증가, 다음 턴 1회에 한해 최대 코스트 감소.
/// </summary>
[CreateAssetMenu(fileName = "SPE_001", menuName = "Create Card Data/Special Card Data/Overclock")]
public class OverclockCardObject : CardObject, ISpecialCardEffect
{
    [SerializeField] private int thisTurnCostBonus = 2;
    [SerializeField] private int nextTurnMaxCostPenalty = 3;

    public void ApplyEffect(PlayerManager player)
    {
        // 이번 턴: 현재 코스트와 최대 코스트를 함께 +2 (AddTurnStatDelta가 turnDeltaStats를 통해 둘 다 반영)
        player.AddTurnStatDelta(StatType.Cost, thisTurnCostBonus);

        // 다음 턴에만 최대 코스트 -3: 즉시 적용하지 않고 예약만 해서 이번 턴에 새어 들어가지 않도록 함
        player.AddPendingNextTurnCostModifier(-nextTurnMaxCostPenalty);
    }
}
