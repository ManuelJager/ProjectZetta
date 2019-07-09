using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon {
    float rateOfFire { get; } //rpm
    bool hasReloaded { get; set; }
    void Fire(Vector2 gridVelocity);
}
