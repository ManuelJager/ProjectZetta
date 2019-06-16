using UnityEngine;

public class PlayerPrefs : MonoBehaviour
{
    /// <summary>
    /// Debugs the shipGrid GameObjectInstanceID and Name of the hit block 
    /// </summary>
    public bool debug1;
    /// <summary>
    /// Debugs the damage stats of the projectile and hit block
    /// </summary>
    public bool debug2;
    /// <summary>
    /// Debugs grid offsets and grid size of a ship
    /// </summary>
    public bool debug3;
    /// <summary>
    /// Debugs count of blocks in a grid
    /// </summary>
    public bool debug4;
    /// <summary>
    /// Debugs the grid positions of multi size blocks 
    /// </summary>
    public bool debug5;
    /// <summary>
    /// Suplementary multi size block debugging
    /// </summary>
    public bool debug6;
    /// <summary>
    /// Debugs grid center of mass;
    /// </summary>
    public bool debug7;
    /// <summary>
    /// Wether or not sabot rounds instantiate shell casing graphics on fire
    /// </summary>
    public bool sabotRoundSpread;
    public bool clearLog;
    /// <summary>
    /// debugs newThrust values
    /// </summary>
    public bool debug8;
    public float thrusterLeniancy;
    public static PlayerPrefs Instance;
    private void Awake()
    {
        Instance = this;
    }
}
