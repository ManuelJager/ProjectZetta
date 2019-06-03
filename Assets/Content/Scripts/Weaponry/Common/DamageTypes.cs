using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DamageTypes
{
    [SerializeField]
    private bool _isExplosive;
    public bool isExplosive => _isExplosive;
    [SerializeField]
    private bool _isKinetic;
    public bool isKinetic => _isKinetic;
    [SerializeField]
    private bool _isEnergetic;
    public bool isEnergetic => _isEnergetic;
    [SerializeField]
    private ExplosiveType _explosiveType;
    public ExplosiveType explosiveType
    {
        get => _explosiveType;
        set => _explosiveType = value;
    }
    [SerializeField]
    private KineticType _kineticType;
    public KineticType kineticType
    {
        get => _kineticType;
        set => _kineticType = value;
    }
    [SerializeField]
    private EnergeticType _energeticType;
    public EnergeticType energeticType
    {
        get => _energeticType;
        set => _energeticType = value;
    }
}
