#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    private int _sourceGridID;
    public int sourceGridID
    {
        get => _sourceGridID;
        set => _sourceGridID = value;
    }
    private float _range;
    public float range => _range;
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
            if (block.blockBaseClass.gridID != sourceGridID)
                ProjectileUtilities.DealDamage(this, block);
    }
}
