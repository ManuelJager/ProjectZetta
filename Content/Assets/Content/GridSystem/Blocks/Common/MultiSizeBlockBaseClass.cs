using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MultiSizeBlockBaseClass
{
    [SerializeField]
    private Vector2Int _size;
    private MonoBehaviour _parentClass;
    public MultiSizeBlockBaseClass(Vector2Int size, MonoBehaviour parentClass)
    {
        _size = size;
        _parentClass = parentClass;
    }
    public Vector2Int size
    {
        get => _size;
        set => _size = value;
    }
    public Vector2Int effectiveSize => parentClass.transform.EffectiveSize(size);
    public MonoBehaviour parentClass
    {
        get => _parentClass;
        set => _parentClass = value;
    }
}
