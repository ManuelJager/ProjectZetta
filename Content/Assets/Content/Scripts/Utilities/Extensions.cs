using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public static class Extensions
{
    /// <summary>
    /// calculates the center of mass of given grid data
    /// </summary>
    /// <param name="posBlockData"></param>
    /// <returns></returns>
    public static Vector2 WeightedAverage(this List<ShipGrid.PosBlockData> posBlockData)
    {
        var weightedXPosValueSum = posBlockData.Sum(x => x.gridPosition.x * x.mass);
        var weightedYPosValueSum = posBlockData.Sum(y => y.gridPosition.y * y.mass);
        var weightSum = posBlockData.Sum(x => x.mass);
        return new Vector2(weightedXPosValueSum / weightSum, weightedYPosValueSum / weightSum);
    }
    /// <summary>
    /// Returns a vector2 array of the gridpositions a multi size block object would occupy
    /// </summary>
    /// <param name="multiSizeBlockObject"></param>
    /// <returns></returns>
    public static Vector2Int[] GetPositionsOfMultiSizeBlock(this ShipGrid.IMultiSizeBlockObject multiSizeBlockObject)
    {
        var size = multiSizeBlockObject.multiSizeBlock.multiSizeBlockBaseClass.effectiveSize;
        var pos = multiSizeBlockObject.transform.localPosition;
        var startingPos = new Vector2Int((int)pos.x - ((size.x - 1) / 2),(int) pos.y - ((size.y - 1) / 2));
        var returnData = new Vector2Int[size.x * size.y];
        int index = 0;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var xPos = startingPos.x + x;
                var yPos = startingPos.y + y;
                returnData[index] = new Vector2Int(xPos, yPos);
                index++;
            }
        }
        #region debugging
        if (PlayerPrefs.Instance.debug5)
        {
            foreach (var item in returnData)
            {
                Debug.Log(item);
            }
        }
        if (PlayerPrefs.Instance.debug6)
        {
            Debug.Log(pos);
            Debug.Log(startingPos);
            Debug.Log("subtraction value is : " + ((size.x - 1) / 2));
        }
        #endregion
        return returnData;
    }
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
    public static void SetThisAsCameraTarget(this Transform transform) => CameraFollower.Instance.SetTarget(transform);

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
    public static IBlock CastToIBlock(this Transform transform) => (IBlock)transform.GetComponent(typeof(IBlock));
    public static IMultiSizeBlock CastToIMultiSizeBlock(this Transform transform) => (IMultiSizeBlock)transform.GetComponent(typeof(IMultiSizeBlock));
    public static Vector2 GetWorldPosCenterOfMassFromGridID(int instanceID)
    {
        var shipObject = GetFromTable(instanceID);
        var shipGrid = shipObject.shipGridClass;
        var shipGridCenterOfMass = shipGrid.centerOfMass;
        var shipGridPos = shipGrid.transform.position;
        var worldPosCenterOfMass = new Vector2(shipGridPos.x + shipGridCenterOfMass.x, shipGridPos.y + shipGridCenterOfMass.y);
        return worldPosCenterOfMass;
    }
    public static Vector2 GetWorldPosCenterOfMassFromGridObject(GameObject shipObject)
    {
        var shipGrid = shipObject.GetComponent<ShipGrid>();
        var shipGridCenterOfMass = shipGrid.centerOfMass;
        var shipGridPos = shipGrid.transform.position;
        var worldPosCenterOfMass = new Vector2(shipGridPos.x + shipGridCenterOfMass.x, shipGridPos.y + shipGridCenterOfMass.y);
        return worldPosCenterOfMass;
    }
    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }
    public static float Effective01RangeMultiplier(float multiplier) => -multiplier + 1;
    public static void RemoveFromGridAndDestroy(this IBlock block)
    {

    }
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
    public static float GetTotalTheoriticalPowerConsumption(List<IPowerConsumer> powerConsumers) => powerConsumers.Sum(item => item.powerConsumption);
    public static float GetTotalTheoriticalPowerGeneration(List<IPowerGenerator> powerGenerators) => powerGenerators.Sum(item => item.powerGeneration);

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

    public static bool IsInRange(this float value, float min, float max) => value > min && value < max;

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
    public static float GetRotation(this Common.Orientation orientation)
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
