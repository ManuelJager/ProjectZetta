#pragma warning disable 649
using UnityEngine;
public class CannonProjectile : MonoBehaviour, IProjectile
{
    private int sourceGridID;
    private float projectileDamage;
    [SerializeField]
    private GameObject frontCasing, backCasing;
    [SerializeField]
    private Rigidbody2D rb2d;
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
        var block = (IBlock)collision.GetComponent(typeof(IBlock));
        if (block == null)
        {
            return;
        }
        ProjectileUtilities.HandlePenetrativeProjectile(ref projectileDamage, gameObject, sourceGridID, collision.transform.GetRootGridID(), block);
    }
}
