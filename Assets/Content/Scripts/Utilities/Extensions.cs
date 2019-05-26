using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Extensions
{
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
    }
}
