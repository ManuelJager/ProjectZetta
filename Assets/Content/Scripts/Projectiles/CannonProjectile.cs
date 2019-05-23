using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : MonoBehaviour, IProjectile
{
    private int sourceGridID;
    public void ApplyDamage()
    {
        throw new System.NotImplementedException();
    }

    public void ProjectileSetup(Quaternion rotation, Transform position, int sourceGridID)
    {
        transform.rotation = rotation;
        transform.position = position.position;
        this.sourceGridID = sourceGridID;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ApplyDamage();
    }
}
