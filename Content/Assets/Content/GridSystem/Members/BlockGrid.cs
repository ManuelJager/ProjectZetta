using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockGrid
{
    private Dictionary<Vector2Int, IBlock> _blocks;

    public int countInGrid
    {
        get
        {
            var list = _blocks.Values.ToList();
            return list.Count;
        }
    }

    public int countOfBlocks
    {
        get
        {
            return blockList.Count;
        }
    }

    public struct Block
    {
        public Transform transform;
        public IBlock block;

        public Block(Transform transform, IBlock block)
        {
            this.transform = transform;
            this.block = block;
        }
    }

    public List<Block> blockList = new List<Block>();

    private int lowestX;
    private int highestX;

    private int lowestY;
    private int highestY;

    private float _mass;
    public float mass
    {
        get
        {
            return _mass;
        }
        private set
        {
            _mass = value;
            shipGrid.rb2d.mass = value;
        }
    }

    private ShipGrid _shipGrid;
    public ShipGrid shipGrid => _shipGrid;

    private Vector2 _centerOfMass;
    public Vector2 centerOfMass
    {
        private set
        {
            var dif = value - -shipGrid.shipLayout.localPosition.ToVector2();

            shipGrid.transform.position       += dif.ToVector3();
            shipGrid.shipLayout.localPosition -= dif.ToVector3();

            _centerOfMass = value;
        }
        get
        {
            UpdateCenterOfMass();
            return _centerOfMass;
        }
    }

    private bool isCenterOfMassUpToDate;

    public BlockGrid(ShipGrid shipGrid, List<IBlock> pBlocks0 = null)
    {
        _blocks = new Dictionary<Vector2Int, IBlock>();
        if (pBlocks0 != null)
            pBlocks0.ForEach(block => AddToGrid(block));
        _shipGrid = shipGrid;
    }

    private Vector2 WeightedAverage()
    {
        var weightedXPosValueSum = blockList.Sum(block => block.transform.localPosition.x * block.block.blockBaseClass.mass);
        var weightedYPosValueSum = blockList.Sum(block => block.transform.localPosition.y * block.block.blockBaseClass.mass);
        var weightSum = blockList.Sum(block => block.block.blockBaseClass.mass);
        isCenterOfMassUpToDate = true;
        return new Vector2(weightedXPosValueSum / weightSum, weightedYPosValueSum / weightSum);
    }

    public void UpdateCenterOfMass()
    {
        if (!isCenterOfMassUpToDate)
            centerOfMass = WeightedAverage();
    }

    public void AddToGrid(IBlock pBlock)
    {
        var pos  = pBlock.blockBaseClass.parentClass.transform.localPosition.ToVector2();
        var size = pBlock.blockBaseClass.effectiveSize;

        var positions = GetPositions(size, pos);

        if (isAvailible(positions))
        {
            blockList.Add(new Block(pBlock.blockBaseClass.transform, pBlock));
            this[positions] = pBlock;
            isCenterOfMassUpToDate = false;
            mass += pBlock.blockBaseClass.mass;
        }
    }

    public void RemoveFromGrid(IBlock pBlock)
    {
        var pos =  pBlock.blockBaseClass.parentClass.transform.localEulerAngles.ToVector2();
        var size = pBlock.blockBaseClass.effectiveSize;

        var positions = GetPositions(size, pos);

        blockList.Remove(blockList.Find(block => block.block == pBlock));
        positions.ForEach(position => _blocks.Remove(position));
        isCenterOfMassUpToDate = false;
        mass -= pBlock.blockBaseClass.mass;
    }

    private List<Vector2Int> GetPositions (Vector2Int size, Vector2 pos)
    {
        List<Vector2Int> inner = new List<Vector2Int>();
        for (int x = 0; x < size.x; x++)
        {
            var effectiveX = pos.x - ((size.x - 1) / 2) + x;
            for (int y = 0; y < size.y; y++)
            {
                var effectiveY = pos.y - ((size.y - 1) / 2) + y;

                var intPos = new Vector2(effectiveX, effectiveY).RoundToInt();
                inner.Add(intPos);
            }
        }
        return inner;
    }

    private bool isAvailible(List<Vector2Int> positions)
    {
        foreach (var position in positions)
        {
            if (this[position.x, position.y] != default)
                return false;
        }
        return true;
    }

    public bool makesContact (IBlock block)
    {
        var pos = getIntPos(block);
        return CheckForNeighbours(pos);
    } 
    /// <summary>
    /// Returns true if there are any neighbouring blocks around the given grid position
    /// </summary>
    /// <param name="pPos">Position in grid</param>
    /// <returns></returns>
    private bool CheckForNeighbours (Vector2Int pPos)
    {
        if (this[pPos + Vector2Int.right] != default)
            return true;
        if (this[pPos + Vector2Int.down] != default)
            return true;
        if (this[pPos + Vector2Int.left] != default)
            return true;
        if (this[pPos + Vector2Int.up] != default)
            return true;
        return false;
    }

    public Vector2Int getIntPos(IBlock block)
    {
        var pos = block.blockBaseClass.parentClass.transform.localPosition;
        var tempPos = pos.ToVector2();
        Vector2Int intPos = new Vector2Int(Mathf.RoundToInt(tempPos.x), Mathf.RoundToInt(tempPos.y));
        return intPos;
    }
    
    public IBlock this[int px, int py]
    {
        get
        {
            if (px > highestX || px < lowestX)
                return default;

            if (py > highestY || py < lowestY)
                return default;

            IBlock block;
            try
            {
                block = _blocks[new Vector2Int(px, py)];
                return block;
            }
            catch { return default; }
            
        }
        private set
        {
            lowestX = px < lowestX ? px : lowestX;
            highestX = px > highestX ? px : highestX;

            lowestY = py < lowestY ? py : lowestY;
            highestY = py > highestY ? py : highestY;

            if (this[px, py] == default)
                _blocks.Add(new Vector2Int(px, py), value);
        }
    }

    private IBlock this[Vector2Int position]
    {
        get
        {
            return this[position.x, position.y];
        }
        set
        {
            this[position.x, position.y] = value;
        }
    }

    private IBlock this[List<Vector2Int> positions]
    {
        set => positions.ForEach(position => this[position] = value);
    }
    
}
