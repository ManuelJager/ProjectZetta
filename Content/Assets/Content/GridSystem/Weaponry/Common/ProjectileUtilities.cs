using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileUtilities
{
    enum damageType
    {
        energetic,
        explosive,
        kinetic
    }
    public static void Initialize(this IProjectile projectile, ProjectileParameters projectileParameters, DamageTypes damageParameters, Vector3 targetPosition, Quaternion targetRotation, Transform projectileTransform, int sourceGridID)
    {
        projectileTransform.position = targetPosition;
        projectileTransform.rotation = targetRotation;
        projectile.damageTypes = damageParameters;
        projectile.projectileReferences.rb2d.AddForce(projectileTransform.right * projectileParameters.speed);
        projectile.sourceGridID = sourceGridID;
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
        var collisionPosition = ((MonoBehaviour)projectile).transform.position;
        if (projectile.damageTypes.isEnergetic)
        {
            DealEnergeticDamage(projectile.damageTypes.energeticType);
        }
        if (projectile.damageTypes.isKinetic)
        {
            DealKineticDamage(projectile.damageTypes.kineticType);
        }
        if (projectile.damageTypes.isExplosive)
        {
            DealExplosiveDamage(projectile.damageTypes.explosiveType, collisionPosition, projectile);
        }
    }
    private static void DealEnergeticDamage(EnergeticType damageParams)
    {
        throw new NotImplementedException();
    }
    private static void DealExplosiveDamage(ExplosiveType damageParams, Vector3 collisionPosition, IProjectile projectile)
    {
        var explosionPosition = collisionPosition.ToVector2();
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(explosionPosition, damageParams.explosiveRadius);
        List<int> gridInstanceIDs = new List<int>();
        //apply explosion knockback to all grids within explosion range
        for (int i = 0; i < colliders2D.Length; i++)
        {
            //casts the collider to an IBlock and then performs a null check so only blocks are damaged
            var colliderBlock = (IBlock)colliders2D[i].transform.GetComponent(typeof(IBlock));
            if (colliderBlock != null)
            {
                //Explosion knockback calculations
                var instanceID = colliderBlock.blockBaseClass.gridID;
                if (!gridInstanceIDs.Contains(instanceID))
                {
                    var shipObject = Extensions.GetFromTable(instanceID);
                    if (shipObject.rb2d == null)
                    {
                        Debug.LogWarning("ship object doesnt exist");
                    }
                    var shipCollider = shipObject.rb2d;
                    var worldPosCenterOfMass = Extensions.GetWorldPosCenterOfMassFromGridID(instanceID);
                    shipCollider.AddForce((worldPosCenterOfMass - explosionPosition) * damageParams.explosiveForce);
                    gridInstanceIDs.Add(instanceID);
                }
                //applies damage
                colliderBlock.ApplyExplosiveDamageToBlock(damageParams.explosiveDamage);
            }
        }
        ((MonoBehaviour)projectile).transform.gameObject.Destroy();

    }
    private static void DealKineticDamage(KineticType damageParams)
    {
        throw new NotImplementedException();
    }
    private static void ApplyExplosiveDamageToBlock(this IBlock block, float damage)
    {
        var armor                     = block.blockBaseClass.armor;
        var blastResistance           = block.blockBaseClass.blastResistance;
        var blastResistanceMultiplier = Extensions.Effective01RangeMultiplier(blastResistance);
        //effective raw damage is the blast damage - armor
        var effectiveRawDamage        = damage - armor;
        //effective damage gets set to a minimum of 0. This is because the armor could be more than the damage dealt, resulting in a negative damage value. Now we don't want to heal the blocks, dow we?
        var effectiveDamage           = effectiveRawDamage < 0f ? 0f : effectiveRawDamage;
        block.blockBaseClass.health  -= effectiveDamage * blastResistanceMultiplier;
    }
}
