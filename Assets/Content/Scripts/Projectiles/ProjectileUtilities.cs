using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class ProjectileUtilities
{
    public enum ProjectileType
    {
        explosive,
        penetrative
    }
    public static void HandlePenetrativeProjectile(ref float projectileDamage, GameObject projectile, int sourceGridID, int colliderGridID, IBlock block)
    {
        //Debugs the shipGrid GameObjectInstanceID and Name of the hit block 
        if (PlayerPrefs.Instance.debug1)
            block.DebugThis();
        if (colliderGridID == sourceGridID)
            //returns out of function if collider grid is the same as the source grid of the projectile
            return;
        //Applies damage to the block
        projectileDamage.ApplyDamageToBlock(block);
        //Substracts projectile health from collision
        projectileDamage.SubtractProjectileHealth(block);
        //Debugs the damage stats of the projectile and hit block
        if (projectileDamage < 0) projectile.Destroy();
    }
    public static void HandleExplosiveProjectile(float explosionDamage, GameObject projectile, int sourceGridID, int colliderGridID, IBlock block, Vector2 explosionPosition, float explosionRadius, float explosionForce)
    {
        //Debugs the shipGrid GameObjectInstanceID and Name of the hit block 
        if (PlayerPrefs.Instance.debug1)
            block.DebugThis();
        if (colliderGridID == sourceGridID)
            //returns out of function if collider grid is the same as the source grid of the projectile
            return;
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius);
        List<int> gridInstanceIDs = new List<int>();
        //apply explosion knockback to all grids within explosion range
        for (int i = 0; i < colliders2D.Length; i++)
        {
            //casts the collider to an IBlock and then performs a null check so only blocks are damaged
            var colliderBlock = colliders2D[i].transform.CastToIBlock();
            if (colliderBlock != null)
            {
                var instanceID = colliders2D[i].transform.GetRootGridID();
                if (!gridInstanceIDs.Contains(instanceID))
                {
                    var shipObject   = Extensions.GetFromTable(instanceID);
                    var shipCollider = shipObject.GetComponent<Rigidbody2D>();
                    var shipGrid     = shipObject.GetComponent<ShipGrid>();
                    var shipGridCenterOfMass = shipGrid.centerOfMass;
                    var shipGridPos = shipGrid.transform.position;
                    var worldPosCenterOfMass = new Vector2(shipGridPos.x + shipGridCenterOfMass.x, shipGridPos.y + shipGridCenterOfMass.y);
                    shipCollider.AddForce((worldPosCenterOfMass - explosionPosition) * explosionForce);
                    gridInstanceIDs.Add(instanceID);
                }
                explosionDamage.ApplyDamageToBlock(block);
            }
            
        }
        projectile.Destroy();
    }
    public static void ApplyDamageToBlock(this float projectileDamage, IBlock block)
    {
        if (PlayerPrefs.Instance.debug2)
        {
            Debug.Log("-BEFORE IMPACT-");
            Debug.Log("ProjectileDamage = " + projectileDamage);
            Debug.Log("BlockHealth = " + block.blockBaseClass.health);
            Debug.Log("BlockArmor = " + block.blockBaseClass.armor);
        }
        var effectiveDamage = projectileDamage - block.blockBaseClass.armor;
        effectiveDamage = effectiveDamage < .0f ? .0f : effectiveDamage;
        block.blockBaseClass.health -= effectiveDamage;
        if (PlayerPrefs.Instance.debug2)
        {
            Debug.Log("-AFTER IMPACT-");
            Debug.Log("ProjectileDamage = " + projectileDamage);
            Debug.Log("BlockHealth = " + block.blockBaseClass.health);
            Debug.Log("BlockArmor = " + block.blockBaseClass.armor);
            Debug.Log("EffectiveDamage = " + effectiveDamage);
            Debug.Log("Hit");
        }
    }
    public static void SubtractProjectileHealth(ref this float projectileDamage, IBlock block) => projectileDamage -= block.blockBaseClass.armor + block.blockBaseClass.health;

}