using UnityEngine;
public interface IProjectile
{
    void ProjectileSetup(Transform rotation, Transform position, float force, float damage, int sourceGridID = 0);
}
