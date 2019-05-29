    #pragma warning disable 649
using UnityEngine;
public class ArmorBlock : MonoBehaviour, IBlock
{
    [SerializeField]
    private float _health;
    [SerializeField]
    private int _mass;
    [SerializeField]
    private int _armor;

    public float health
    {
        get => _health;
        set
        { 
            _health = value;
            if (value <= .0f)
            {
                SubtractFromGridAndDestroy();
            }
        }
    }
    public int mass => _mass;
    public int armor => _armor;
    public void SubtractFromGridAndDestroy()
    {
        Destroy(gameObject);
    }
    public void DebugThis()
    {
        Debug.Log("Rood id of " + transform.root.name + " is : " + transform.GetRootGridID());
    }
}
