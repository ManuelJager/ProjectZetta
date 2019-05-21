using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private float turretSpeed, projectileSpeed, fireRate;
    [SerializeField]
    private GameObject turret;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject firePoint;
    [SerializeField]
    private AudioSource audioSource;
    private void Update()
    {
        turret.transform.rotation = RotationUtilities.MouseLookAtRotation(turret, turretSpeed);
        if (Input.GetMouseButton(0) && GetCanFire())
        {
            animator.SetTrigger("Fire");
        }
        Debug.Log(GetCanFire());
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
}
