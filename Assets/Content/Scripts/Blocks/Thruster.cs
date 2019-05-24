#pragma warning disable 649
using UnityEngine;
public class Thruster : MonoBehaviour, IBlock, IThruster
{
    #region vars
    [SerializeField]
    private float health;
    [SerializeField]
    private int mass;
    [SerializeField]
    private int armor;
    [SerializeField]
    private float thrust;
    [SerializeField]
    private GameObject thruster;
    #endregion
    public Thruster(float health, int mass, int armor, float thrust)
    {
        this.health = health;
        this.mass = mass;
        this.armor = armor;
        this.thrust = thrust;
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
    public void SetThrusterFlame(bool value, float strength = 0f)
    {
        thruster.SetActive(value);
    }
    public void SetThrust(float value)
    {
        thrust = value;
    }
    public float GetThrust()
    {
        return thrust;
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
