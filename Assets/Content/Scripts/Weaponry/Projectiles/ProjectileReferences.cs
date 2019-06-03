using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ProjectileReferences
{
    [SerializeField]
    private Rigidbody2D _rb2d;
    public Rigidbody2D rb2d => _rb2d;
}
