#pragma warning disable 649
using System.Collections;
using UnityEngine;
public class Turret : MonoBehaviour, ITurret, IBlock
{
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
    private Transform _turretObject;
    [SerializeField]
    private BlockBaseClass _blockBaseClass;
    [SerializeField]
    private DamageTypes _damageTypes;
    public DamageTypes damageTypes => _damageTypes;
    public BlockBaseClass blockBaseClass
    {
        get => _blockBaseClass;
        set => _blockBaseClass = value;
    }
    public bool hasReloaded {
        get => _hasReloaded;
        set => _hasReloaded = value;
    }
    public float rateOfFire => _rateOfFire;
    public float turretSpeed => _turretSpeed;
    public Transform turretObject => _turretObject;
    [SerializeField]
    private ProjectileParameters projectileParameters;

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
