using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    /// <param name="SourceGridID">ID of the grid where this projectile was fired from to prevent a projectile from hitting its own grid, or in case of friendly fire being disabled, its own team members</param>
    void ApplyDamage();
    void ProjectileSetup(Quaternion rotation, Transform position, int sourceGridID);
}
