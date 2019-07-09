#pragma warning disable 649
using UnityEngine;
public class SmallGyroscope : MonoBehaviour, IBlock, IGyroscope
{
    [SerializeField]
    private BlockBaseClass _blockBaseClass;
    public BlockBaseClass blockBaseClass
    {
        get => _blockBaseClass;
        set => _blockBaseClass = value;
    }
    [SerializeField]
    private float _gyroForce;
    public float gyroForce => _gyroForce;
}
