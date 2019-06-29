using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurret : IWeapon
{
    Transform turretObject { get; }
    float turretSpeed { get; }
}
