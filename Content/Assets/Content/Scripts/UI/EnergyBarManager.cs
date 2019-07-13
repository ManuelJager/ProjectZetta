using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnergyBarManager : MonoBehaviour, IPanel
{
    public bool rawEnabled => gameObject.activeInHierarchy;

    private bool _enabled; 
    public bool enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            if (value == _enabled)
                return;
            if (value)
                Enable();
            else
                Disable();
        }
    }

    public bool ready => enabled || !rawEnabled;

    public void Disable()
    {
    }

    public void Enable()
    {
    }

    public void ImmedeateDisable()
    {
    }

    public void ImmideateEnable()
    {
    }
}
