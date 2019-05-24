using UnityEngine;

public class PlayerPrefs : MonoBehaviour
{
    public bool debug1;
    public bool debug2;
    public static PlayerPrefs Instance;
    private void Start()
    {
        Instance = this;
    }
}
