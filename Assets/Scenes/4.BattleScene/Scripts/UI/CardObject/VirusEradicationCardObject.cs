using UnityEngine;

/// <summary>
/// SPE_006 바이러스 박멸(VIRUS ERADICATION): 현재 스폰된 모든 적에게 방어력을 무시하는 고정 피해를 입힌다.
/// </summary>
[CreateAssetMenu(fileName = "SPE_006", menuName = "Create Card Data/Special Card Data/Virus Eradication")]
public class VirusEradicationCardObject : CardObject, ISpecialCardEffect
{
    [SerializeField] private int trueDamage = 10;

    public void ApplyEffect(PlayerManager player)
    {
        if (VirusSpawn.instance == null) return;

        foreach (Transform slot in VirusSpawn.instance.spawns)
        {
            if (slot.childCount == 0) continue;

            Virus virus = slot.GetChild(0).GetComponent<Virus>();
            if (virus != null)
            {
                virus.ApplyTrueDamage(trueDamage);
            }
        }
    }
}
