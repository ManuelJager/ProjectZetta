public interface IBlock
{
    float health { get; set; }
    int mass { get; }
    int armor { get; }
    void SubtractFromGridAndDestroy();
    void DebugThis();
}
