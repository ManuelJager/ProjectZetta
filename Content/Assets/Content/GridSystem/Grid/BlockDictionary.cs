using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDictionary : MonoBehaviour
{
    [SerializeField]
    private GameObject LightArmorBlock;
    [SerializeField]
    private GameObject FighterCockpit;
    [SerializeField]
    private GameObject SmallGyroscope;
    [SerializeField]
    private GameObject SmallThruster;
    [SerializeField]
    private GameObject MediumThruster;
    [SerializeField]
    private GameObject AutoCannon;
    [SerializeField]
    private GameObject Cannon;

    public static BlockDictionary Instance;

    private Dictionary<int, GameObject> _dictionary;

    public GameObject this [int index]
    {
        get
        {
            try
            {
                var go = _dictionary[index];
                return go;
            }
            catch
            {
                Debug.LogWarning("invalid index key of value " + index);
                return null;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        _dictionary = new Dictionary<int, GameObject>()
        {
            { 0, LightArmorBlock },
            { 10, FighterCockpit},
            { 20, SmallGyroscope},
            { 30, SmallThruster},
            { 31, MediumThruster},
            { 40, AutoCannon},
            { 41, Cannon}
        };
    }
}
