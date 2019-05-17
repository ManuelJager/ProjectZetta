using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock
{
    void SetHealth(float value);
    float GetHealth();
    int GetMass();
    int GetArmor();
    void Destroy();
}
