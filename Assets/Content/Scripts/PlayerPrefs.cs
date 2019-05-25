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
    /// Wether or not sabot rounds instantiate shell casing graphics on fire
    /// </summary>
    public bool sabotRoundSpread;
    public static PlayerPrefs Instance;
    private void Awake()
    {
        Instance = this;
    }
}
