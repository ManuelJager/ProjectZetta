using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollower : MonoBehaviour
{
    private GameObject _target;
    public void SetTarget(GameObject target)
    {
        _target = target;
    }
    private void Update()
    {
        if (_target != null)
        {
            transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, -10);
        }
    }
}
