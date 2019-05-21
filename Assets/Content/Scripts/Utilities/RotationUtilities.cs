using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RotationUtilities
{
    /// <summary>
    /// Rotates a target towards the mouse at a constant rate
    /// Works with independent left / right turning rates
    /// This feature is intended in the case of ship rotation, to take into account assymetric thruster layouts, for a more realistic feel
    /// </summary>
    /// <param name="target">Target GameObject</param>
    /// <param name="camera">Rendering camera</param>
    /// <param name="leftTurningRate">Speed at which the object should turn to the left</param>
    /// <param name="rightTurningRate">Speed at which the object should turn to the right</param>
    /// <returns></returns>
    public static Quaternion MouseLookAtRotation(GameObject target, float leftTurningRate, float rightTurningRate, Camera camera = null)
    {
        camera = camera ?? Camera.main;
        Quaternion q = GetMouseWorldPos(target, camera);
        var zStep = CalculateZStep(target.transform.rotation.eulerAngles, q.eulerAngles, leftTurningRate, rightTurningRate);
        return Quaternion.Euler(target.transform.rotation.eulerAngles + new Vector3(.0f, .0f, zStep));
    }
    /// <summary>
    /// Rotates a target towards the mouse at a constant rate
    /// </summary>
    /// <param name="target">Target GameObject</param>
    /// <param name="camera">Rendering camera</param>
    /// <param name="turningRate">Speed at which the object should turn to any side</param>
    public static Quaternion MouseLookAtRotation(GameObject target, float turningRate, Camera camera = null)
    {
        camera = camera ?? Camera.main;
        Quaternion q = GetMouseWorldPos(target, camera);
        var zStep = CalculateZStep(target.transform.rotation.eulerAngles, q.eulerAngles, turningRate);
        return Quaternion.Euler(target.transform.rotation.eulerAngles + new Vector3(.0f, .0f, zStep));
    }
    /// <summary>
    /// Rotates an object towards a target at a constant rate
    /// Works with independent left / right turning rates
    /// This feature is intended in the case of ship rotation, to take into account assymetric thruster layouts, for a more realistic feel
    /// </summary>
    /// <param name="current">The gameObject that needs to rotate</param>
    /// <param name="target">Target GameObject</param>
    /// <param name="leftTurningRate">Speed at which the object should turn to the left</param>
    /// <param name="rightTurningRate">Speed at which the object should turn to the right</param>
    /// <returns></returns>
    public static Quaternion ObjectLookAtRotation(GameObject current, GameObject target, float leftTurningRate, float rightTurningRate)
    {
        Quaternion q = GetRotationToTarget(current, target);
        var zStep = CalculateZStep(target.transform.rotation.eulerAngles, q.eulerAngles, leftTurningRate, rightTurningRate);
        return Quaternion.Euler(target.transform.rotation.eulerAngles + new Vector3(.0f, .0f, zStep));
    }
    /// <summary>
    /// Rotates an object towards a target at a constant rate
    /// </summary>
    /// <param name="current">The gameObject that needs to rotate</param>
    /// <param name="target">Target GameObject</param>
    /// <param name="turningRate">Speed at which the object should turn to any side</param>
    /// <returns></returns>
    public static Quaternion ObjectLookAtRotation(GameObject current, GameObject target, float turningRate)
    {
        Quaternion q = GetRotationToTarget(current, target);
        var zStep = CalculateZStep(current.transform.rotation.eulerAngles, q.eulerAngles, turningRate);
        return Quaternion.Euler(current.transform.rotation.eulerAngles + new Vector3(.0f, .0f, zStep));
    }
    /// <summary>
    /// gets rotation relative from target to mouse position
    /// </summary>
    private static Quaternion GetMouseWorldPos(GameObject target, Camera camera)
    {
        Vector3 vectorToTarget = camera.ScreenToWorldPoint(Input.mousePosition) - target.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
    /// <summary>
    /// gets rotation relative from current to target
    /// </summary>
    private static Quaternion GetRotationToTarget(GameObject current, GameObject target)
    {
        Vector3 vectorToTarget = current.transform.position - target.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
    /// <summary>
    /// Calculates the z angle step linearly
    /// <para>
    /// Prevents over rotation
    /// Calculates z step size based on the left / right rotation rate
    /// </para>
    /// </summary>
    /// <param name="targetRotation">Rotation from object to target</param>
    /// <param name="currentRotation">Rotation of object</param>
    /// <param name="leftTurningRate">Speed at which the object should turn to the left</param>
    /// <param name="rightTurningRate">Speed at which the object should turn to the right</param>
    /// <returns></returns>
    private static float CalculateZStep(Vector3 targetRotation, Vector3 currentRotation, float leftTurningRate, float rightTurningRate)
    {
        //zDif is smallest value betweens the two z rotation differences
        float zDif = GetZDif(targetRotation, currentRotation);
        //wether to rotate left(true) or right(false)
        bool isTargetRotationLeftToRotation = IsTargetRotationLeftToRotation(targetRotation, currentRotation);
        //angle in degrees the rotation should added uppon
        float zStep = isTargetRotationLeftToRotation ? leftTurningRate : rightTurningRate * -1;
        zStep *= Time.deltaTime;
        //the effective value of zStep
        //this line makes sure the zStep doesn't exceed the zDif. This is to prevent the rotation from overshooting its target
        float effectiveZStep = Mathf.Abs(zStep) > zDif ? isTargetRotationLeftToRotation ? zDif : zDif * -1f : zStep;
        return effectiveZStep;
    }
    /// <summary>
    /// Calculates the z angle step linearly
    /// <para>
    /// Prevents over rotation
    /// </para>
    /// </summary>
    /// <param name="targetRotation">Rotation from object to target</param>
    /// <param name="currentRotation">Rotation of object</param>
    /// <param name="turningRate">Speed at which the object should turn to any side</param>
    /// <returns></returns>
    private static float CalculateZStep(Vector3 targetRotation, Vector3 currentRotation, float turningRate)
    {
        float zDif = GetZDif(targetRotation, currentRotation);
        //wether to rotate left(true) or right(false)
        bool isTargetRotationLeftToRotation = IsTargetRotationLeftToRotation(targetRotation, currentRotation);
        //angle in degrees the rotation should added uppon
        float zStep = isTargetRotationLeftToRotation ? turningRate : turningRate * -1;
        zStep *= Time.deltaTime;
        //the effective value of zStep
        //this line makes sure the zStep doesn't exceed the zDif. This is to prevent the rotation from overshooting its target
        float effectiveZStep = Mathf.Abs(zStep) > zDif ? isTargetRotationLeftToRotation ? zDif : zDif * -1f : zStep;
        return effectiveZStep;
    }
    /// <summary>
    /// returns wether the target rotation is to the left or to the right of the current rotation
    /// </summary>
    private static bool IsTargetRotationLeftToRotation(Vector3 targetRotation, Vector3 currentRotation)
    {
        bool rotateDirection = ((targetRotation.z - currentRotation.z + 360f) % 360f) > 180.0f ? true : false;
        return rotateDirection;
    }
    /// <summary>
    /// returns the smallest difference between the two vector 3 rotations along the z axis
    /// </summary>
    private static float GetZDif(Vector3 targetRotation, Vector3 currentRotation)
    {
        float zDif1 = Mathf.Abs(targetRotation.z - currentRotation.z);
        float zDif2 = Mathf.Abs(currentRotation.z - targetRotation.z);
        float zDif = Mathf.Min(zDif1, zDif2);
        return zDif;
    }
    /// <summary>
    /// Rotates a vector2 along the z axis
    /// </summary>
    public static Vector2 RotateVector2(Vector2 vector, float angle)
    {
        #region magic
        var theta = angle * Mathf.Deg2Rad;

        var cs = Mathf.Cos(theta);
        var sn = Mathf.Sin(theta);

        var px = vector.x * cs - vector.y * sn;
        var py = vector.x * sn + vector.y * cs;
        #endregion
        return new Vector2(px, py);
    }
}
