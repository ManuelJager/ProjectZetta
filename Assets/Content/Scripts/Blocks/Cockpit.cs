#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockpit : MonoBehaviour, IMultiSizeBlock
{
    [SerializeField]
    private float _health;
    [SerializeField]
    private int _mass;
    [SerializeField]
    private int _armor;
    [SerializeField]
    private Vector2Int _size;

    public float health
    {
        get => _health;
        set
        {
            _health = value;
            if (value <= .0f) SubtractFromGridAndDestroy();
        }
    }
    public int mass => _mass;
    public int armor => _armor;
    public Vector2Int size {
        get => _size;
        set => _size = value;
    }
    public Vector2Int effectiveSize => transform.EffectiveSize(size);
    public void SubtractFromGridAndDestroy()
    {
        Destroy(gameObject);
    }
    public void DebugThis()
    {
        Debug.Log("Rood id of " + transform.root.name + " is : " + transform.GetRootGridID());
    }
}
