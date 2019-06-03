using System;
using System.Collections;
using UnityEngine;

public static class ProjectileUtilities
{
    public static void Initialize(this IProjectile projectile, ProjectileParameters projectileParameters, DamageTypes damageParameters, Vector3 targetPosition, Quaternion targetRotation, Transform projectileTransform)
    {
        projectileTransform.position = targetPosition;
        projectileTransform.rotation = targetRotation;
        projectile.damageTypes = damageParameters;
        projectile.projectileReferences.rb2d.AddForce(projectileTransform.right * projectileParameters.speed);
        /*
        Type type = projectile.GetType();
        var fields = type.GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].Name == "range")
            {
                Debug.Log("field found");
                fields[i].SetValue(projectile, projectileParameters.range);
            }
        }*/
        
        
    }
    public static void DealDamage(IProjectile projectile, IBlock block)
    {
        if (projectile.damageTypes.isEnergetic)
        {
            DealEnergeticDamage(projectile.damageTypes.energeticType);
        }
        if (projectile.damageTypes.isExplosive)
        {
            DealExplosiveDamage(projectile.damageTypes.explosiveType);
        }
        if (projectile.damageTypes.isKinetic)
        {
            DealKineticDamage(projectile.damageTypes.kineticType);
        }
    }
    private static void DealEnergeticDamage(EnergeticType damageParams)
    {
        throw new NotImplementedException();
    }
    private static void DealExplosiveDamage(ExplosiveType damageParams)
    {
        throw new NotImplementedException();
    }
    private static void DealKineticDamage(KineticType damageParams)
    {
        throw new NotImplementedException();
    }
}
