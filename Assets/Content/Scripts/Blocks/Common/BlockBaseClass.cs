using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BlockBaseClass
{
    [SerializeField]
    private float _health;
    [SerializeField]
    private int _mass;
    [SerializeField]
    private int _armor;
    private IBlock _block;
    private MonoBehaviour _parentClass;
    private int _gridID;
    public BlockBaseClass(float health, int mass, int armor, IBlock block, MonoBehaviour parentClass)
    {
        _health = health;
        _mass = mass;
        _armor = armor;
        _block = block;
        _parentClass = parentClass;
    }
    public float health
    {
        get => _health;
        set
        {
            _health = value;
            if (value <= .0f)
            {
                block.SubtractFromGridAndDestroy();
            }
        }
    }
    public IBlock block
    {
        get => _block;
        set => _block = value;
    }
    public MonoBehaviour parentClass
    {
        get => _parentClass;
        set => _parentClass = value;
    }
    public int mass => _mass;
    public int armor => _armor;
    public int gridID
    {
        get => _gridID;
        set => _gridID = value;
    }
}
