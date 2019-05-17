using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBlock : MonoBehaviour, IBlock
{
    private float health;
    private int mass;
    private int armor;
    public ArmorBlock(float health, int mass, int armor)
    {
        this.health = health;
        this.mass = mass;
        this.armor = armor;
    }
    public void SetHealth(float value)
    {
        health -= value;
        if (health <= .0f)
        {
            Destroy();
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
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
