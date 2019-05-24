public interface IBlock : IGridMember
{
    void SetHealth(float value);
    float GetHealth();
    int GetMass();
    int GetArmor();
    void SubtractFromGridAndDestroy();
    void DebugThis();
}
