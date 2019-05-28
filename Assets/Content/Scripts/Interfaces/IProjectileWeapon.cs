using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileWeapon
{
    float projectileSpeed { get; }
    float projectilePenetration { get; }
}
