#pragma warning disable 649
using UnityEngine;
public class AutoCannonProjectile : MonoBehaviour, IExplosiveProjectile
{
    private int sourceGridID;
    private float projectileDamage;
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private float _radius;
    [SerializeField]
    private float _explosionForce;
    public float radius
    {
        get => _radius;
        set => _radius = radius;
    }
    public void ProjectileSetup(Transform rotation, Transform position, float force, float projectileDamage, int sourceGridID = 0)
    {
        transform.rotation = rotation.rotation;
        transform.position = position.position;
        this.sourceGridID = sourceGridID;
        this.projectileDamage = projectileDamage;
        rb2d.AddForce(transform.right * force);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var block = collision.transform.CastToIBlock();
        if (block == null)
            return;
        ProjectileUtilities.HandleExplosiveProjectile(projectileDamage, gameObject, sourceGridID, collision.transform.GetRootGridID(), block, transform.position, radius, _explosionForce);
    }
}
