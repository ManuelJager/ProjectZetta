#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockpit : MonoBehaviour, IMultiSizeBlock, IPowerGenerator, IGyroscope
{
    [SerializeField]
    private MultiSizeBlockBaseClass _multiSizeBlockBaseClass;
    [SerializeField]
    private BlockBaseClass _blockBaseClass;

    public BlockBaseClass blockBaseClass
    {
        get => _blockBaseClass;
        set => _blockBaseClass = value;
    }
    public MultiSizeBlockBaseClass multiSizeBlockBaseClass
    {
        get => _multiSizeBlockBaseClass;
        set => _multiSizeBlockBaseClass = value;
    }
    [SerializeField]
    private float _powerGeneration;
    public float powerGeneration => _powerGeneration;
    [SerializeField]
    private float _gyroForce;
    public float gyroForce => _gyroForce;

    public void SubtractFromGridAndDestroy()
    {
        Destroy(gameObject);
    }
    public void DebugThis()
    {
        Debug.Log("Rood id of " + transform.root.name + " is : " + transform.GetRootGridID());
    }
}
