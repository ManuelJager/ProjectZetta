#pragma warning disable 649
using UnityEngine;
public class AutoCannonProjectile : MonoBehaviour, IExplosiveProjectile, IProjectile
{
    private ExplosiveDamageType _explosiveDamage;
    private BaseProjectileType _baseProjectile;
    public ExplosiveDamageType explosiveDamage
    {
        get => _explosiveDamage;
        set => _explosiveDamage = value;
    }
    public BaseProjectileType baseProjectile
    {
        get => _baseProjectile;
        set => _baseProjectile = value;
    } 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var block = collision.transform.CastToIBlock();
        if (block == null)
            return;
        ProjectileUtilities.HandleExplosiveProjectile(baseProjectile, explosiveDamage, block.blockBaseClass.gridID, block, transform.position);
    }
}
