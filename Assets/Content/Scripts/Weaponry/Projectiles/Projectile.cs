using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    public int sourceGridID;
    public float range;
    [Header("DamageTypes")]
    private DamageTypes _damageTypes;
    public DamageTypes damageTypes
    {
        get => _damageTypes;
        set => _damageTypes = value;
    }
    [Header("ProjectileReferences")]
    [SerializeField]
    private ProjectileReferences _projectileReferences;
    public ProjectileReferences projectileReferences => _projectileReferences;
    [SerializeField]
    private ProjectileParameters _projectileParameters;
    public ProjectileParameters projectileParameters
    {
        get => _projectileParameters;
        set => _projectileParameters = value;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var block = (IBlock)collision.GetComponent(typeof(IBlock));
        if (block != null)
            ProjectileUtilities.DealDamage(this, block);
    }
}
