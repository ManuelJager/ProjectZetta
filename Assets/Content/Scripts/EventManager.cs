using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public static class EventManager
{
    public delegate void CameraTarget(Transform target);
    public static event CameraTarget CameraTargetEvent;

    public static void CallCameraTargetEvent(Transform target)
    {
        CameraTargetEvent?.Invoke(target);
    }
}
