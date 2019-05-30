using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{ 
    public static GridManager Instance;

    public Hashtable gridInstances;
    private void Awake()
    {
        Instance = this;
        gridInstances = new Hashtable();
    }
}
