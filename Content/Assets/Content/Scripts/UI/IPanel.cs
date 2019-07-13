using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPanel
{
    void Enable();
    void Disable();
    void ImmideateEnable();
    void ImmedeateDisable();
    bool rawEnabled { get; }
    bool enabled { get; set; }
    bool ready { get; }
}
