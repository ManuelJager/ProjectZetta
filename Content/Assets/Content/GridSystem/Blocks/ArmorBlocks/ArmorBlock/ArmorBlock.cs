    #pragma warning disable 649
using UnityEngine;
public class ArmorBlock : MonoBehaviour, IBlock
{
    [SerializeField]
    private BlockBaseClass _blockBaseClass;
    public BlockBaseClass blockBaseClass
    {
        get => _blockBaseClass;
        set => _blockBaseClass = value;
    }
}
