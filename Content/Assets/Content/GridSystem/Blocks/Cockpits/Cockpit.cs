#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockpit : MonoBehaviour, IBlock, IPowerGenerator, IGyroscope
{
    [SerializeField]
    private BlockBaseClass _blockBaseClass;

    public BlockBaseClass blockBaseClass
    {
        get => _blockBaseClass;
        set => _blockBaseClass = value;
    }
    [SerializeField]
    private float _powerGeneration;
    public float powerGeneration => _powerGeneration;
    [SerializeField]
    private float _gyroForce;
    public float gyroForce => _gyroForce;
}
