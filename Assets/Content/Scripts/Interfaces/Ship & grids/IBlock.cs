public interface IBlock
{
    BlockBaseClass blockBaseClass { get; set; }
    void SubtractFromGridAndDestroy();
    void DebugThis();
}
