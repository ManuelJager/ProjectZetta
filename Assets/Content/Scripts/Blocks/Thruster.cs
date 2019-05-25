#pragma warning disable 649
using UnityEngine;
public class Thruster : MonoBehaviour, IBlock, IThruster
{
    [SerializeField]
    private float _health;
    [SerializeField]
    private int _mass;
    [SerializeField]
    private int _armor;
    [SerializeField]
    private float thrust;
    [SerializeField]
    private GameObject thruster;
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (value <= .0f)
            {
                SubtractFromGridAndDestroy();
            }
        }
    }
    public int mass
    {
        get
        {
            return _mass;
        }
    }

    public int armor
    {
        get
        {
            return _armor;
        }
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
