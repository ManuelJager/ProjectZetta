#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _target;
    [SerializeField]
    private Camera _camera;
    public static CameraManager Instance;

    private float minSize = 20;
    private float maxSize = 50;
    private float _targetSize;

    public float targetSize
    {
        get => _targetSize;
        set => _targetSize = value > maxSize ? maxSize : value < minSize ? minSize : value;
    }

    public float currentSize
    {
        get => _camera.orthographicSize;
        set => _camera.orthographicSize = value;
    }

    public Transform target
    {
        set => _target = value;
    }

    private void Awake() => Instance = this;
    
    private void Update()
    {
        HandleCameraTracking();
        HandleCameraSize();
    }

    public void HandleCameraTracking()
    {
        if (_target != null)
            transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, -10);
    }

    public void HandleCameraSize()
    {
        targetSize -= Input.mouseScrollDelta.y;
        var size = currentSize;
        Extensions.MixedInterpolate(ref size, targetSize, 0.01f, 0.01f);
        _camera.orthographicSize = size;
        if (!PlayerPrefs.Instance.lightWeightMode)
            BackgroundManager.Instance.globalScale = currentSize / 20;
    }
}
