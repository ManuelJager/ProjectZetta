#pragma warning disable 649
using System.Collections;
using UnityEngine;
public class Turret : MonoBehaviour, IGridMember
{
    [SerializeField]
    public float turretSpeed, projectileSpeed, projectileDamage, fireRate;
    [SerializeField]
    public GameObject turret;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject firePoint;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private GameObject projectilePrefab;
    private bool allowFire = true;
    public void Fire()
    {
        if (allowFire) StartCoroutine(FireShot(fireRate));
    }
    private IEnumerator FireShot (float rpm)
    {
        allowFire = false;
        animator.SetTrigger("Fire");
        var projectile = Instantiate(projectilePrefab);
        var IProjectile = (IProjectile)projectile.GetComponent(typeof(IProjectile));
        IProjectile.ProjectileSetup(transform, firePoint.transform, projectileSpeed, projectileDamage, GetRootGridID());
        yield return new WaitForSeconds(60.0f / rpm);
        allowFire = true;
    }
    public void SetCanFireTrue()
    {
        animator.SetBool("CanFire", true);
    }
    public void SetCanFireFalse()
    {
        animator.SetBool("CanFire", false);
    }
    public bool GetCanFire()
    {
        return animator.GetBool("CanFire");
    }
    public void PlayShot()
    {
        audioSource.Play();
    }
    public int GetRootGridID()
    {
        return transform.root.GetInstanceID();
    }
}
