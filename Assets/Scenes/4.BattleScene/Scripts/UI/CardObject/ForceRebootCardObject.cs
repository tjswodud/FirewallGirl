using UnityEngine;

/// <summary>
/// SPE_005 강제 재부팅(FORCE REBOOT): 아군에게 적용된 모든 버프/디버프를 제거하고,
/// 스폰된 모든 적의 상태 효과 아이콘을 숨긴다.
/// </summary>
[CreateAssetMenu(fileName = "SPE_005", menuName = "Create Card Data/Special Card Data/Force Reboot")]
public class ForceRebootCardObject : CardObject, ISpecialCardEffect
{
    public void ApplyEffect(PlayerManager player)
    {
        player.ClearAllStatusEffects();

        if (VirusSpawn.instance == null) return;

        foreach (Transform slot in VirusSpawn.instance.spawns)
        {
            if (slot.childCount == 0) continue;

            Virus virus = slot.GetChild(0).GetComponent<Virus>();
            if (virus == null || virus.enemyUIController == null) continue;

            virus.enemyUIController.enemyStatusUI?.ClearAllEffects();
        }
    }
}
