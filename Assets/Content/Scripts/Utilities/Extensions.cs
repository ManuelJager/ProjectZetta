using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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

    public static Vector2[] getPositionsOfMultiSizeBlock(this ShipGrid.IMultiSizeBlockObject multiSizeBlockObject)
    {
        var size = multiSizeBlockObject.multiSizeBlock.effectiveSize;
        var pos = multiSizeBlockObject.transform.localPosition;
        var startingPos = new Vector2(pos.x - ((size.x - 1) / 2), pos.y - ((size.y - 1) / 2));
        var returnData = new Vector2[size.x * size.y];
        int index = 0;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var xPos = startingPos.x + x;
                var yPos = startingPos.y + y;
                returnData[index] = new Vector2(xPos, yPos);
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
    public static int GetRootGridID(this Transform transform) => transform.root.GetInstanceID();
}
