using UnityEngine;
public interface IProjectile
{
    DamageTypes damageTypes { get; set; }
    ProjectileReferences projectileReferences { get; }
    ProjectileParameters projectileParameters { get; set; }
}
