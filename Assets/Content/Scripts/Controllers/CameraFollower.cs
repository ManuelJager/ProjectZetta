using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollower : MonoBehaviour
{
    private Transform _target;
    public void SetTarget(Transform target)
    {
        _target = target;
        Debug.Log("TargetSet");
    }
    private void Update()
    {
        if (_target != null)
        {
            transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, -10);
        }
    }

    private void OnEnable()
    {
        EventManager.CameraTargetEvent += SetTarget;
    }

    private void OnDisable()
    {
        EventManager.CameraTargetEvent -= SetTarget;
    }
}
