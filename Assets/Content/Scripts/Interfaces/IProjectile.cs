using UnityEngine;
public interface IProjectile
{
    /// <param name="SourceGridID">ID of the grid where this projectile was fired from to prevent a projectile from hitting its own grid, or in case of friendly fire being disabled, its own team members</param>
    void ApplyDamage(int colliderGridID, IBlock block);
    void ProjectileSetup(Transform rotation, Transform position, float force, float damage, int sourceGridID = 0);
}
