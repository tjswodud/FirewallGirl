using UnityEngine;

/// <summary>
/// SPE_004 무적의 방화벽(INVINCIBLE FIREWALL): 다음 턴에 받는 모든 공격 피해를 무효화한다.
/// </summary>
[CreateAssetMenu(fileName = "SPE_004", menuName = "Create Card Data/Special Card Data/Invincible Firewall")]
public class InvincibleFirewallCardObject : CardObject, ISpecialCardEffect
{
    public void ApplyEffect(PlayerManager player)
    {
        player.ActivateNextEnemyTurnDamageImmunity();
    }
}
