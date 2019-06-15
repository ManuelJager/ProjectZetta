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
    /// <param name="q">Rotation from object to mouse position</param>
    /// <returns></returns>
    public static Quaternion MouseLookAtRotation(Transform target, float leftTurningRate, float rightTurningRate, Quaternion? q = null, Camera camera = null)
    {
        camera = camera ?? Camera.main;
        q = q ?? GetMouseWorldPos(target, camera);
        Quaternion effectiveQ = q ?? default;
        var zStep = CalculateZStep(target.rotation.eulerAngles, effectiveQ.eulerAngles, leftTurningRate, rightTurningRate);
        return Quaternion.Euler(target.rotation.eulerAngles + new Vector3(.0f, .0f, zStep));
    }
    /// <summary>
    /// Rotates a target towards the mouse at a constant rate
    /// </summary>
    /// <param name="target">Target GameObject</param>
    /// <param name="camera">Rendering camera</param>
    /// <param name="turningRate">Speed at which the object should turn to any side</param>
    public static Quaternion MouseLookAtRotation(Transform target, float turningRate, Camera camera = null)
    {
        camera = camera ?? Camera.main;
        Quaternion q = GetMouseWorldPos(target, camera);
        var zStep = CalculateZStep(target.rotation.eulerAngles, q.eulerAngles, turningRate);
        return Quaternion.Euler(target.rotation.eulerAngles + new Vector3(.0f, .0f, zStep));
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
    public static Quaternion GetMouseWorldPos(Transform target, Camera camera = null)
    {
        camera = camera ?? Camera.main;
        Vector3 vectorToTarget = camera.ScreenToWorldPoint(Input.mousePosition) - target.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
    /// <summary>
    /// gets rotation relative from current to target
    /// </summary>
    public static Quaternion GetRotationToTarget(GameObject current, GameObject target)
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
        float zStep = isTargetRotationLeftToRotation ? leftTurningRate : -rightTurningRate;
        zStep *= Time.deltaTime;
        //the effective value of zStep
        //this line makes sure the zStep doesn't exceed the zDif. This is to prevent the rotation from overshooting its target
        float effectiveZStep = Mathf.Abs(zStep) > zDif ? isTargetRotationLeftToRotation ? zDif : -zDif : zStep;
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
        float zStep = isTargetRotationLeftToRotation ? turningRate : -turningRate;
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
    public static Common.Orientation GetOrientation(this IBlock block)
    {
        if (block.blockBaseClass.orientation != null)
            return block.blockBaseClass.orientation ?? default;
        var rotation = ((MonoBehaviour)block).transform.rotation;
        var orientation = GetOrientation(rotation);
        block.blockBaseClass.orientation = orientation;
        return orientation;
    }
    private static Common.Orientation GetOrientation(Quaternion rotation)
    {
        switch (Mathf.RoundToInt(rotation.eulerAngles.z))
        {
            case 0:
                return Common.Orientation.forward;
            case 90:
                return Common.Orientation.left;
            case 180:
                return Common.Orientation.backward;
            case 270:
                return Common.Orientation.right;
            default:
                throw new System.ArgumentException("Rotation is invalid");
        }
    }
    public static Common.Orientation GetOrientation(float zRotation)
    {
        switch (zRotation)
        {
            case 0f:
                return Common.Orientation.forward;
            case 90f:
                return Common.Orientation.right;
            case 180f:
                return Common.Orientation.backward;
            case 270f:
                return Common.Orientation.left;
            default:
                throw new System.ArgumentException("Rotation is invalid");
        }
    }
    public static Common.Orientation GetOrientation(char keyPressed)
    {
        switch (keyPressed)
        {
            case 'w':
                return Common.Orientation.forward;
            case 'a':
                return Common.Orientation.left;
            case 's':
                return Common.Orientation.backward;
            case 'd':
                return Common.Orientation.right;
            default:
                throw new System.ArgumentException("KeyPressed is invalid");
        }
    }
    public static float GetRotation(Common.Orientation orientation)
    {
        switch (orientation)
        {
            case Common.Orientation.forward:
                return 0f;
            case Common.Orientation.backward:
                return 180f;
            case Common.Orientation.left:
                return 270f;
            case Common.Orientation.right:
                return 90f;
            default:
                throw new System.ArgumentException("Orientation Invalid");
        }
    }
    public static void AddAngle(this ref float angle, float addition)
    {
        angle += addition;
            if (angle > 360f)
            {
                angle -= 360f;
            }
            else if (angle < 0f)
            {
                angle += 360f;
            }
    }
}