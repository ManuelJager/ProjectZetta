using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class KineticDamageType
{
    private float _kineticDamage;
    private float _penetrativeForce;
    private float _impactDamage;
    private float _effectiveKineticDamage;
    public KineticDamageType(float kineticDamage, float penetrativeForce, float impactDamage)
    {
        _kineticDamage = kineticDamage;
        _penetrativeForce = penetrativeForce;
        _impactDamage = impactDamage;
    }
    public float kineticDamage => _kineticDamage;
    public float penetrativeForce => _penetrativeForce;
    public float impactDamage => _impactDamage;
    public float effectiveKineticDamage
    {
        get => _effectiveKineticDamage;
        set => _effectiveKineticDamage = value;
    }
}
