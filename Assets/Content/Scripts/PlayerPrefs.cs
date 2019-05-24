using UnityEngine;

public class PlayerPrefs : MonoBehaviour
{
    public bool debug1;
    public bool debug2;
    public bool debug3;
    public bool sabotRoundSpread;
    public static PlayerPrefs Instance;
    private void Awake()
    {
        Instance = this;
    }
}
