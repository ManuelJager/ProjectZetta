#pragma warning disable 649
using System.Collections;
using UnityEngine;
public class Turret : MonoBehaviour, ITurret, IBlock
{
    [SerializeField]
    public float projectileSpeed, projectileDamage;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject firePoint;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private GameObject projectilePrefab;
    private bool _hasReloaded = true;
    [SerializeField]
    private float _rateOfFire;
    [SerializeField]
    private float _turretSpeed;
    [SerializeField]
    private float _health;
    [SerializeField]
    private int _mass;
    [SerializeField]
    private int _armor;
    [SerializeField]
    private Transform _turretObject;
    public float health
    {
        get => _health;
        set
        {
            _health = value;
            if (value <= .0f)
            {
                SubtractFromGridAndDestroy();
            }
        }
    }
    public int mass => _mass;
    public int armor => _armor;
    public bool hasReloaded {
        get => _hasReloaded;
        set => _hasReloaded = value;
    }
    public float rateOfFire => _rateOfFire;
    public float turretSpeed => _turretSpeed;
    public Transform turretObject => _turretObject;
    public void Fire()
    {
        if (hasReloaded) StartCoroutine(FireShot(rateOfFire));
    }
    private IEnumerator FireShot (float rpm)
    {
        hasReloaded = false;
        animator.SetTrigger("Fire");
        PlayShot();
        var projectile = Instantiate(projectilePrefab);
        var IProjectile = (IProjectile)projectile.GetComponent(typeof(IProjectile));
        IProjectile.ProjectileSetup(turretObject, firePoint.transform, projectileSpeed, projectileDamage, transform.GetRootGridID());
        yield return new WaitForSeconds(60.0f / rpm);
        hasReloaded = true;
    }
    public void PlayShot()
    {
        audioSource.Play();
    }
    public void SubtractFromGridAndDestroy()
    {
        Destroy(gameObject);
    }
    public void DebugThis()
    {
        Debug.Log("Rood id of " + transform.root.name + " is : " + transform.GetRootGridID());
    }
}
