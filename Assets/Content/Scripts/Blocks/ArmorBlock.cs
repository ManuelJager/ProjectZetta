using UnityEngine;
public class ArmorBlock : MonoBehaviour, IBlock
{
    #region vars
    [SerializeField]
    private float health;
    [SerializeField]
    private int mass;
    [SerializeField]
    private int armor;
    #endregion
    public ArmorBlock(float health, int mass, int armor)
    {
        this.health = health;
        this.mass = mass;
        this.armor = armor;
    }
    public void SetHealth(float value)
    {
        health = value;
        if (health <= .0f)
        {
            SubtractFromGridAndDestroy();
        }
    }
    public float GetHealth()
    {
        return health;
    }
    public int GetMass()
    {
        return mass;
    }
    public int GetArmor()
    {
        return armor;
    }
    public void SubtractFromGridAndDestroy()
    {
        Destroy(gameObject);
    }
    public int GetRootGridID()
    {
        return transform.root.GetInstanceID();
    }
    public void DebugThis()
    {
        Debug.Log("Rood id of " + transform.root.name + " is : " + GetRootGridID());
    }
}
