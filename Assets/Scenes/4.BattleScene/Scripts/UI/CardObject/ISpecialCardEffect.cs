/// <summary>
/// "적용" 버튼 클릭 즉시 발동하는 특수 효과 카드가 구현하는 인터페이스.
/// 일반 카드처럼 조합 대기열(sequenceQueue)을 거치지 않는다.
/// </summary>
public interface ISpecialCardEffect
{
    void ApplyEffect(PlayerManager player);
}
