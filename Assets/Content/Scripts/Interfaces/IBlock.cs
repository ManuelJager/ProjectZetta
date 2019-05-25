public interface IBlock : IGridMember
{
    float health { get; set; }
    int mass { get; }
    int armor { get; }
    void SubtractFromGridAndDestroy();
    void DebugThis();
}
