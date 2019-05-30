using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileUtilities
{
    public enum ProjectileType
    {
        explosive,
        penetrative
    }
    public static void HandlePenetrativeProjectile(ref float projectileDamage, GameObject gameObject, int sourceGridID, int colliderGridID, IBlock block)
    {
        //Debugs the shipGrid GameObjectInstanceID and Name of the hit block 
        if (PlayerPrefs.Instance.debug1) block.DebugThis();
        if (colliderGridID == sourceGridID)
        {
            //returns out of function if collider grid is the same as the source grid of the projectile
            return;
        }
        projectileDamage.ApplyDamageToBlock(block, ProjectileType.penetrative);
        //Debugs the damage stats of the projectile and hit block
        if (projectileDamage < 0)
        {
            GameObject.Destroy(gameObject);
            return;
        }
    }
    public static void ApplyDamageToBlock(ref this float projectileDamage, IBlock block, ProjectileType projectileType, float? blastRadius = null)
    {
        var blockHealth = block.health;
        var blockArmor = block.armor;
        var effectiveDamage = projectileDamage - blockArmor;
        var effectiveTotalHealth = blockHealth + blockArmor;
        effectiveDamage = effectiveDamage < .0f ? .0f : effectiveDamage;
        switch (projectileType)
        {
            case ProjectileType.explosive:

                break;
            case ProjectileType.penetrative:
                projectileDamage -= effectiveTotalHealth;

                break;
            default:
                break;
        }
        block.health = blockHealth - effectiveDamage;
        if (PlayerPrefs.Instance.debug2)
        {
            Debug.Log("ProjectileDamage = " + projectileDamage);
            Debug.Log("BlockHealth = " + blockHealth);
            Debug.Log("BlockArmor = " + blockArmor);
            Debug.Log("EffectiveDamage = " + effectiveDamage);
            Debug.Log("Hit");
        }
    }

}