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
    public Vector2Int size
    {
        get
        {
            return _size;
        }
        set
        {
            _size = value;
        }
    }
    public Vector2Int effectiveSize
    {
        get
        {
            var zRot = transform.rotation.eulerAngles.z;
            //Checks if block has been rotated, if so returns a size vector that takes the rotation into consideration
            if (zRot == 90f || zRot == 270f)
            {
                return new Vector2Int(size.y, size.x);
            }
            return size;
        }
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
