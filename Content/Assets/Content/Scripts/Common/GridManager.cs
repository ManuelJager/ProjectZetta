using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{ 
    public struct shipReference
    {
        public GameObject grid;
        public ShipGrid shipGridClass;
        public Rigidbody2D rb2d;
        public shipReference(GameObject grid, ShipGrid shipGridClass, Rigidbody2D rb2d)
        {
            this.grid = grid;
            this.shipGridClass = shipGridClass;
            this.rb2d = rb2d;
        }
    }
    public static GridManager Instance;

    public Dictionary<int, shipReference> gridInstances;
    private void Awake()
    {
        Instance = this;
        gridInstances = new Dictionary<int, shipReference>();
    }

}
