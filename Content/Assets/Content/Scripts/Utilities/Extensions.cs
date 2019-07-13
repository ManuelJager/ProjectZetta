using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public static class Extensions
{
    /// <summary>
    /// Gets the instance id of the absolute parent of a give transform
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static int GetRootGridID(this Transform transform) => transform.root.gameObject.GetInstanceID();
    /// <summary>
    /// Returns the effective size of a block inside the grid based on the block rotation
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static Vector2Int EffectiveSize(this Transform transform, Vector2Int size)
    {
        var zRot = transform.rotation.eulerAngles.z;
        //Checks if block has been rotated, if so returns a size vector that takes the rotation into consideration
        if (zRot == 90f || zRot == 270f)
        {
            return new Vector2Int(size.y, size.x);
        }
        return size;
    }
    /// <summary>
    /// Sets this transform as the target of the camera follower instance
    /// </summary>
    /// <param name="transform"></param>
    public static void SetThisAsCameraTarget(this Transform transform) => CameraManager.Instance.target = transform;

    public static void AddToTable(this GridManager.shipReference shipReference, int instanceID)
    {
        try
        {
            GridManager.Instance.gridInstances.Add(instanceID, shipReference);
        }
        catch
        {
            Debug.LogWarning("grid : " + instanceID + " already exists");
        }
    }
    public static void RemoveFromTable(this GridManager.shipReference shipReference)
    {
        var instanceID = shipReference.grid.transform.GetRootGridID();
        try
        {
            GridManager.Instance.gridInstances.Remove(instanceID);
        }
        catch
        {
            Debug.LogWarning("grid : " + instanceID + " doesn't exist");
        }
    }
    public static GridManager.shipReference GetFromTable(int instanceID)
    {

        GridManager.shipReference? shipReference = new GridManager.shipReference?();
        try
        {
            shipReference = (GridManager.shipReference)GridManager.Instance.gridInstances[instanceID];
            if (shipReference == null)
                Debug.LogWarning("Ship grid : " + instanceID + " is null");
        }
        catch
        {
            Debug.LogWarning("grid : " + instanceID + " doesn't exist");
        }
        return shipReference ?? default; ;
    }
    public static void Destroy(this GameObject gameObject) => UnityEngine.Object.Destroy(gameObject);
    public static Vector2 GetWorldPosCenterOfMassFromGridID(int instanceID)
    {
        var shipObject = GetFromTable(instanceID);
        var shipGrid = shipObject.shipGridClass;
        var shipGridCenterOfMass = shipGrid.blockGrid.centerOfMass;
        var shipGridPos = shipGrid.transform.position;
        var worldPosCenterOfMass = new Vector2(shipGridPos.x + shipGridCenterOfMass.x, shipGridPos.y + shipGridCenterOfMass.y);
        return worldPosCenterOfMass;
    }
    public static Vector2 GetWorldPosCenterOfMassFromGridObject(GameObject shipObject)
    {
        var shipGrid = shipObject.GetComponent<ShipGrid>();
        var shipGridCenterOfMass = shipGrid.blockGrid.centerOfMass;
        var shipGridPos = shipGrid.transform.position;
        var worldPosCenterOfMass = new Vector2(shipGridPos.x + shipGridCenterOfMass.x, shipGridPos.y + shipGridCenterOfMass.y);
        return worldPosCenterOfMass;
    }
    public static Vector2 ToVector2(this Vector3 vector) => new Vector2(vector.x, vector.y);
    public static float Effective01RangeMultiplier(float multiplier) => -multiplier + 1;
    public static Vector3 ToVector3(this Vector2 vector, float zValue = 0f) => new Vector3(vector.x, vector.y, zValue);
    /// <summary>
    /// for loop wrapper that repeats an action an amount of time
    /// </summary>
    /// <param name="count"></param>
    /// <param name="action"></param>
    public static void Times(this int count, Action action)
    {
        try
        {
            var a = (uint)count;
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }
        catch 
        {
            throw new ArgumentException("extension parameter of Extensions.Times() must be a non-negative value");
        }
        
    }

    public static bool IsPositive (this float value) => value >= 0f;


    public static void MixedInterpolate(this ref float from, float to, float multiplier = 0.5f, float step = 0.5f)
    {
        if (from == to)
            return;
        var dif = to - from;
        if (dif == 0f)
            return;
        from += dif * multiplier;
        dif = to - from;
        from += dif > 0 ? dif > step ? step : dif : dif < -step ? -step : dif;
    }

    public static void MixedInterpolatev2(this ref Vector2 from, Vector2 to, float multiplier = 0.5f, float step = 0.5f)
    {
        var value = from;
        value.x.MixedInterpolate(to.x, multiplier, step);
        value.y.MixedInterpolate(to.y, multiplier, step);
        from = value;
    }

    public static void MixedInterpolatev3(this ref Vector3 from, Vector3 to, float multiplier = 0.5f, float step = 0.5f)
    {
        var value = from;
        value.x.MixedInterpolate(to.x, multiplier, step);
        value.y.MixedInterpolate(to.y, multiplier, step);
        value.z.MixedInterpolate(to.z, multiplier, step);
        from = value;
    }

    public static bool IsInRange(this float value, float min, float max) => value > min && value < max;

    public static Vector2Int GetOrientation(float zRot)
    {
        switch (zRot)
        {
            case 0f:  return Vector2Int.right;
            case 90f: return Vector2Int.up;
            case 180: return Vector2Int.left;
            case 270: return Vector2Int.down;
            default:
                Debug.LogError("Invalid rotation given");
                return Vector2Int.zero;
        }
    }

    public static float GetOrientation(Vector2Int orientation)
    {
        if (orientation == Vector2Int.right) return 0f;
        if (orientation == Vector2Int.up)    return 90f;
        if (orientation == Vector2Int.left)  return 180f;
        if (orientation == Vector2Int.down)  return 270f;
        Debug.LogError("Invalid orientation given");
        return 0f;
    }

    public static int GetOrientationIndex(Vector2Int orientation)
    {
        if (orientation == Vector2Int.right) return 0;
        if (orientation == Vector2Int.up)    return 1;
        if (orientation == Vector2Int.left)  return 2;
        if (orientation == Vector2Int.down)  return 3;
        Debug.LogError("Invalid orientation given");
        return 0;
    }

    public static Vector2Int GetOrientationByIndex(int orientationIndex)
    {
        if (orientationIndex == 0) return Vector2Int.right;
        if (orientationIndex == 1) return Vector2Int.up;
        if (orientationIndex == 2) return Vector2Int.left;
        if (orientationIndex == 3) return Vector2Int.down;
        Debug.LogError("Invalid index given");
        return Vector2Int.zero;
    }

    public static float GetRotation(this Vector2Int orientation)
    {
        var val = Mathf.Rad2Deg * Mathf.Atan2(orientation.x, orientation.y) + 90f;
        val.AddAngleRef(180f);
        return val;
    }

    public static void AddAngleRef(this ref float angle, float addition)
    {
        angle += addition;
        if (angle > 360f)
            angle -= 360f;
        
        else if (angle < 0f)
            angle += 360f;
    }
    public static float AddAngle(this float angle, float addition)
    {
        angle += addition;
        if (angle > 360f)
            angle -= 360f;
        
        else if (angle < 0f)
            angle += 360f;
        
        return angle;
    }
    /// <summary>
    /// Returns true if the given angle is between the range of the min and max
    /// </summary>
    public static bool AngleIsInRange(this float angle, float min, float max)
    {
        angle = (360 + (angle % 360)) % 360;
        min = (3600000 + min) % 360;
        max = (3600000 + max) % 360;

        if (min < max)
            return min <= angle && angle <= max;
        return min <= angle || angle <= max;
    }
    /// <summary>
    /// Returns true if the given angle is between the range of the min and max
    /// </summary>
    public static bool AngleIsInRange(this float angle, NewThrust.MinMax minMax)
    {
        var min = minMax.min;
        var max = minMax.max;
        angle = (360 + (angle % 360)) % 360;
        min = (3600000 + min) % 360;
        max = (3600000 + max) % 360;

        if (min < max)
            return min <= angle && angle <= max;
        return min <= angle || angle <= max;
    }
    public static Vector2Int RoundToInt(this Vector2 vector) => new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));

    public static ColorBlock MultiplyColorBlockByAlpha(ColorBlock colorBlock, float alpha)
    {
        var block = colorBlock;
        var color = new Color[4];

        color[0] = colorBlock.highlightedColor;
        color[1] = colorBlock.normalColor;
        color[2] = colorBlock.pressedColor;
        color[3] = colorBlock.selectedColor;

        for (int i = 0; i < 4; i++)
            color[i].a *= alpha;

        block.highlightedColor = color[0];
        block.normalColor = color[1];
        block.pressedColor = color[2];
        block.selectedColor = color[3];

        return block;
    }
}

public class ManuQueue<T> : IEnumerable
{
    protected private LinkedList<T> _inner;
    private int _size;
    public int size
    {
        get => _size;
        set
        {
            if (value < 1)
                Debug.LogWarning("Size is too small");
            else
            {
                _inner = new LinkedList<T>();
                _size = value;
            }
        }
    }
    public void Enqueue(T item)
    {
        _inner.AddFirst(item);
        if (_inner.Count > size)
            (_inner.Count - size).Times(() => _inner.RemoveLast());
    }
    public T Peek() => _inner.Count > 0 ? _inner.ElementAt(0) : default;
    public T Dequeue()
    {
        var t = _inner.ElementAt(0);
        _inner.RemoveFirst();
        return t;
    }
    public IEnumerator GetEnumerator() {
        foreach (var item in _inner)
            yield return item;
    }
    public ManuQueue(int size = 1) => this.size = size;
    public T this[int index]
    {
        get
        {
            try
            {
                var t = _inner.ElementAt(index);
                return t;
            }
            catch
            {
                throw new ArgumentException("Index was out of ManuQueue range");
            }
        }
    }
}
