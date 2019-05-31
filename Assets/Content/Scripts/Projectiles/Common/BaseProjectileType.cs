using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class BaseProjectileType
{
    private int _sourceGridID;
    private Rigidbody2D _rb2d;
    private MonoBehaviour _parentClass;
    public BaseProjectileType(int sourceGridID, Rigidbody2D rb2d, MonoBehaviour parentClass, Transform rotation, Transform position, float force)
    {
        _sourceGridID = sourceGridID;
        _rb2d = rb2d;
        _parentClass = parentClass;
        parentClass.transform.position = position.position;
        parentClass.transform.rotation = rotation.rotation;
        _rb2d.AddForce(parentClass.transform.right * force);
    }
    public int sourceGridID => _sourceGridID;
    public Rigidbody2D rb2d => _rb2d;
    public MonoBehaviour parentClass => _parentClass;
}
