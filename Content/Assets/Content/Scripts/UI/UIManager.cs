using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIBar energyBar;
    public static UIManager Instance;

    private void Awake() => Instance = this;
}
