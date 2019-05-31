using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class ExplosiveDamageType : MonoBehaviour
{
    private float _explosionDamage;
    private float _explosionForce;
    private float _explosionRadius;
    public ExplosiveDamageType(float explosionDamage, float explosionForce, float explosionRadius)
    {
        _explosionDamage = explosionDamage;
        _explosionForce = explosionForce;
        _explosionRadius = explosionRadius;
    }
    public float explosionDamage => _explosionDamage;
    public float explosionForce => _explosionForce;
    public float explosionRadius => _explosionRadius;
}
