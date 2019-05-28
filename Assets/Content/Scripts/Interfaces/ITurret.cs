using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurret : IWeapon
{
    GameObject turretObject { get; }
    float turretSpeed { get; }
}
