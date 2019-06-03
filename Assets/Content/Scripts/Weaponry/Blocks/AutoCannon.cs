#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCannon : MonoBehaviour, IBlock, ITurret, IWeapon
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject firePoint;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private BlockBaseClass _blockBaseClass;
    public BlockBaseClass blockBaseClass
    {
        get => _blockBaseClass;
        set => _blockBaseClass = value;
    }
    [SerializeField]
    private Transform _turretObject; 
    public Transform turretObject => _turretObject;
    [SerializeField]
    private float _turretSpeed;
    public float turretSpeed => _turretSpeed;
    [SerializeField]
    private float _rateOfFire;
    public float rateOfFire => _rateOfFire;
    private bool _hasReloaded = true;
    public bool hasReloaded
    {
        get => _hasReloaded;
        set => _hasReloaded = value;
    }
    public ProjectileParameters projectileParameters;
    [SerializeField]
    private DamageTypes _damageTypes;
    public DamageTypes damageTypes => _damageTypes;

    public void DebugThis()
    {
        throw new System.NotImplementedException();
    }

    public void Fire()
    {
        if (hasReloaded)
            StartCoroutine(FireShot());
    }

    public IEnumerator FireShot()
    {
        hasReloaded = false;
        animator.SetTrigger("Fire");
        PlayShot();
        var projectile = Instantiate(projectilePrefab);
        var IProjectile = (IProjectile)projectile.GetComponent(typeof(IProjectile));
        IProjectile.Initialize(projectileParameters, damageTypes, firePoint.transform.position, turretObject.rotation, projectile.transform, blockBaseClass.gridID);
        yield return new WaitForSeconds(60.0f / rateOfFire);
        hasReloaded = true;
    }
    public void PlayShot()
    {
        audioSource.Play();
    }

    public void SubtractFromGridAndDestroy()
    {
        
    }
}
