using UnityEngine;
public class CannonProjectile : MonoBehaviour, IProjectile
{
    private int sourceGridID;
    private float projectileDamage;
    public void ApplyDamage(int colliderGridID, IBlock block)
    {
        
        //Debugs the shipGrid GameObjectInstanceID and Name of the hit block 
        if (PlayerPrefs.Instance.debug1) block.DebugThis();
        if (colliderGridID == sourceGridID)
        {
            //returns out of function if collider grid is the same as the source grid of the projectile
            return;
        }
        var blockHealth = block.GetHealth();
        var blockArmor = block.GetArmor();
        var effectiveDamage = projectileDamage - blockArmor;
        var effectiveTotalHealth = blockHealth + blockArmor;
        //Debugs the damage stats of the projectile and hit block
        if (PlayerPrefs.Instance.debug2)
        {
            Debug.Log("ProjectileDamage = " + projectileDamage);
            Debug.Log("BlockHealth = " + blockHealth);
            Debug.Log("BlockArmor = " + blockArmor);
            Debug.Log("EffectiveDamage = " + effectiveDamage);
            Debug.Log("Hit");
        }
        if (effectiveDamage > blockArmor) block.SetHealth(blockHealth - effectiveDamage);
        projectileDamage -= effectiveTotalHealth;
        if (projectileDamage < 0)
        {
            Destroy(gameObject);
            return;
        }
    }
    public void ProjectileSetup(Transform rotation, Transform position, float force, float projectileDamage, int sourceGridID = 0)
    {
        transform.rotation = rotation.rotation;
        transform.position = position.position;
        this.sourceGridID = sourceGridID;
        this.projectileDamage = projectileDamage;
        GetComponent<Rigidbody2D>().AddForce(transform.right * force);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var block = (IBlock)collision.GetComponent(typeof(IBlock));
        if (block == null)
        {
            return;
        }
        ApplyDamage(block.GetRootGridID(), block);
    }
}
