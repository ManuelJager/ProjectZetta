using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BackgroundManager backgroundManager;

    private void Awake()
    {
        Instance = this;
    }
}
