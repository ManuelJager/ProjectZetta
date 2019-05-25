using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMultiSizeBlock : IBlock
{
    Vector2Int size { get; set; }
}
