using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThruster
{
    void SetThrusterFlame(bool value, float strength = 0f);
    void SetThrust(float value);
    float GetThrust();
}
