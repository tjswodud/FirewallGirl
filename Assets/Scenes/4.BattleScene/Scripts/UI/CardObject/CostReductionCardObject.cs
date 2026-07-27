using UnityEngine;

/// <summary>
/// SPE_003 비용 절감(COST REDUCTION): 다음 턴에 사용하는 모든 카드의 코스트 소모 -1, 해당 턴 이후 원상복구.
/// </summary>
[CreateAssetMenu(fileName = "SPE_003", menuName = "Create Card Data/Special Card Data/Cost Reduction")]
public class CostReductionCardObject : CardObject, ISpecialCardEffect
{
    [SerializeField] private int nextTurnCostDiscountPerCard = 1;

    public void ApplyEffect(PlayerManager player)
    {
        player.AddPendingNextTurnCostDiscount(nextTurnCostDiscountPerCard);
    }
}
