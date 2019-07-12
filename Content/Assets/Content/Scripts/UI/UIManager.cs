using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _energyBar;
    [SerializeField]
    private GameObject _pauseMenu;

    private PanelDict panelDict;

    public enum UIState
    {
        IN_GAME,
        IN_GAME_PAUSED,
        IN_MAIN_MENU,
        NULL
    }

    public UIState currentState = UIState.NULL;

    public void switchState(UIState to)
    {
        if (to == currentState)
            return;

        switch (currentState)
        {
            case UIState.IN_GAME:
                panelDict[_energyBar].Disable();
                break;
            case UIState.IN_GAME_PAUSED:
                panelDict[_pauseMenu].Disable();
                break;
            case UIState.IN_MAIN_MENU:
                break;
            default:
                break;
        }

        switch (to)
        {
            case UIState.IN_GAME:
                panelDict[_energyBar].Enable();
                break;
            case UIState.IN_GAME_PAUSED:
                panelDict[_pauseMenu].Enable();
                break;
            case UIState.IN_MAIN_MENU:
                break;
            default:
                break;
        }

        currentState = to;

        Debug.Log($@"Switched UIState to {to}");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentState)
            {
                case UIState.IN_GAME:
                    switchState(UIState.IN_GAME_PAUSED);
                    break;
                case UIState.IN_GAME_PAUSED:
                    switchState(UIState.IN_GAME);
                    break;
                default:
                    break;
            }
        } 
    }

    public void Start()
    {
        panelDict = new PanelDict();
        panelDict.Add(_energyBar, _pauseMenu);
        panelDict.DisableAllImmediate();
        switchState(UIState.NULL);
        switchState(UIState.IN_GAME);
    }


    public UIBar energyBar;
    public static UIManager Instance;

    private void Awake() => Instance = this;

    public class PanelDict
    {
        private Dictionary<GameObject, IPanel> _internalPanelDict;

        public IPanel this [GameObject key]
        {
            get
            {
                try
                {
                    return _internalPanelDict[key];
                }
                catch
                {
                    Debug.LogError("Unsassigned Panel");
                    return default;
                }
            }
        }

        public void Add(GameObject key)
        {
            var panel = (IPanel)key.GetComponent(typeof(IPanel));
            if (panel != null)
                _internalPanelDict.Add(key, panel);
        }

        public void Add(params GameObject[] keys) => keys.ToList().ForEach(key => Add(key));

        public void DisableAllImmediate() => _internalPanelDict.ToList().ForEach(panel => panel.Value.ImmedeateDisable());

        public void DisableAll() => _internalPanelDict.ToList().ForEach(panel => panel.Value.Disable());

        public PanelDict()
        {
            _internalPanelDict = new Dictionary<GameObject, IPanel>();
        }
    }
}
