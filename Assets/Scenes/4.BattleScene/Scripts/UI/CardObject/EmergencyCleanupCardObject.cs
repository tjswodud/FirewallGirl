using UnityEngine;

/// <summary>
/// SPE_002 긴급 정리(EMERGENCY CLEANUP): 손에 있는 카드를 모두 버리고 3장을 새로 뽑는다.
/// </summary>
[CreateAssetMenu(fileName = "SPE_002", menuName = "Create Card Data/Special Card Data/Emergency Cleanup")]
public class EmergencyCleanupCardObject : CardObject, ISpecialCardEffect
{
    private const int RedrawCount = 3;

    public void ApplyEffect(PlayerManager player)
    {
        player.DiscardHandAndRedraw(RedrawCount);
    }
}
