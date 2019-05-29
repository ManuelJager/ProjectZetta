using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosiveProjectileWeapon
{
    float explosionRadius { get; }
    float explosionDamage { get; }
}
