#pragma warning disable 649
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GridUtilities.GridReader;

public class ShipGrid : MonoBehaviour
{
    public struct PosBlockData
    {
        public Vector2 gridPosition;
        public float mass;

        public PosBlockData(Vector2 gridPosition, float mass)
        {
            this.gridPosition = gridPosition;
            this.mass = mass;
        }
    }
    public struct IBlockObject
    {
        public IBlock block;
        public Transform transform;

        public IBlockObject(IBlock block, Transform transform)
        {
            this.block = block;
            this.transform = transform;
        }
    }
    public struct IMultiSizeBlockObject
    {
        public IMultiSizeBlock multiSizeBlock;
        public Transform transform;

        public IMultiSizeBlockObject(IMultiSizeBlock multiSizeBlock, Transform transform)
        {
            this.multiSizeBlock = multiSizeBlock;
            this.transform = transform;
        }
    }

    private GameObject _ship;
    public Rigidbody2D _rb2d;
    private GameObject[,] _shipGrid;
    private Vector2Int lowest;
    private Vector2Int highest;

    public List<ITurret> turrets = new List<ITurret>();

    public List<IPowerConsumer> powerConsumers = new List<IPowerConsumer>();
    public List<IPowerGenerator> powerGenerators = new List<IPowerGenerator>();

    [HideInInspector]
    public float totalPowerGeneration;
    [HideInInspector]
    public float totalPowerConsumption;

    [SerializeField]
    private Transform _shipLayout;
    public Transform shipLayout => _shipLayout != null ? _shipLayout : _shipLayout = transform.GetChild(0).GetChild(0);
    [SerializeField]
    private Transform _grid;
    public Transform grid => _grid != null ? _grid : _grid = transform.GetChild(0);

    public struct TurningRate
    {
        private float _turningRate;
        public float turningRate => _turningRate;

        public TurningRate(float turningRate = 0.0f)
        {
            _turningRate = new float();
        }
        public void Add(IGyroscope gyroscope)
        {
            _turningRate += gyroscope.gyroForce;
        }
        public void Remove(IGyroscope gyroscope)
        {
            _turningRate += gyroscope.gyroForce;
        }
    }
    public TurningRate turningRate;

    public Vector2 centerOfMass;

    public int gridID;

    public NewThrust newThrust;

    [SerializeField]
    private MonoBehaviour _controller;
    public IController controller;

    public BlockGrid blockGrid = new BlockGrid();

    private void Start()
    {
        _ship = gameObject;
        _rb2d = GetComponent<Rigidbody2D>();
        gridID = transform.GetRootGridID();
        newThrust = new NewThrust(this);
        turningRate = new TurningRate();
        new GridManager.shipReference(gameObject, this, _rb2d).AddToTable(gridID);
        ConstructGrid();
        controller = (IController)_controller;
    }
    public void AddToGrid(Transform block)
    {
        var pos = block.localPosition;
        var targetXIndex = (int)pos.x - lowest.x;
        var targetYIndex = (int)pos.y - lowest.y;
        if (!IndexIsOutsideGridBounds(new int[] { targetXIndex, targetYIndex }))
        {
            if (_shipGrid[targetXIndex, targetYIndex] != null)
            {
                Debug.LogWarning("ShipGrid at : " + new Vector2(targetXIndex, targetYIndex) + " is already assigned by " + _shipGrid[targetXIndex, targetYIndex].name + ", name of block is : " + block.name);
            }
            else
            {
                _shipGrid[targetXIndex, targetYIndex] = block.gameObject;
            }
        }
        else
        {
            Debug.LogWarning("Block is outside of bounds");
        }
    }
    public void AddToGrid(Transform block, Vector2 posInGrid)
    {
        var pos = posInGrid;
        var targetXIndex = (int)pos.x - lowest.x;
        var targetYIndex = (int)pos.y - lowest.y;
        if (!IndexIsOutsideGridBounds(new int[] { targetXIndex, targetYIndex }))
        {
            if (_shipGrid[targetXIndex, targetYIndex] != null)
            {
                Debug.LogWarning("ShipGrid at : " + new Vector2(targetXIndex, targetYIndex) + " is already assigned by " + _shipGrid[targetXIndex, targetYIndex].name + ", name of block is : " + block.name);
            }
            else
            {
                _shipGrid[targetXIndex, targetYIndex] = block.gameObject;
            }
        }
        else
        {
            Debug.LogWarning("Block is outside of bounds");
        }
    }
    public void AddStatsToGrid(Transform pBlock)
    {
        var component = pBlock.GetComponent(typeof(IBlock));

        if (component == null)
        {
            Debug.LogWarning("block : " + pBlock.name + " could not be cast to IBlock");
            return;
        }

        var block = (IBlock)component;

        if (pBlock.root != transform)
        {
            Debug.LogWarning("Cannot add stats of block : " + pBlock.name + " to grid : " + transform.GetInstanceID() + " because the block is not inside the hirearchy of this grid.");
            return;
        }

        block.blockBaseClass.gridID = gridID;

        block.blockBaseClass.parentClass = (MonoBehaviour)component;

        try
        {
            var multisizeBlock = (IMultiSizeBlock)component;
            multisizeBlock.multiSizeBlockBaseClass.parentClass = (MonoBehaviour)component;
        }
        catch { }

        try
        {
            var turret = (ITurret)component;
            if (turret != null)
            {
                turrets.Add(turret);
            }
        }
        catch { }

        try
        {
            var thruster = (IThruster)component;
            if (thruster != null)
            {
                newThrust.Add(thruster);
            }
        }
        catch { }

        try
        {
            var gyroscope = (IGyroscope)component;
            if (gyroscope != null)
            {
                turningRate.Add(gyroscope);
            }
        }
        catch { }

        blockGrid.AddToGrid(block);
    }
    public void RemoveStatsFromGrid(Transform pBlock)
    {
        var component = pBlock.GetComponent(typeof(IBlock));

        if (component == null)
        {
            Debug.LogWarning("block : " + pBlock.name + " could not be cast to IBlock");
            return;
        }

        var block = (IBlock)component;

        if (pBlock.root != transform)
        {
            Debug.LogWarning("Cannot remove stats of block : " + pBlock.name + " to grid : " + transform.GetInstanceID() + " because the block is not inside the hirearchy of this grid.");
            return;
        }

        try
        {
            var multisizeBlock = (IMultiSizeBlock)component;
        }
        catch { }

        try
        {
            var turret = (ITurret)component;
            if (turret != null)
            {
                turrets.Remove(turret);
            }
        }
        catch { }

        try
        {
            var thruster = (IThruster)component;
            if (thruster != null)
            {
                newThrust.Remove(thruster);
            }
        }
        catch { }

        try
        {
            var gyroscope = (IGyroscope)component;
            if (gyroscope != null)
            {
                turningRate.Remove(gyroscope);
            }
        }
        catch { }



    }
    public void RemoveFromGrid(Transform pBlock)
    {
        RemoveStatsFromGrid(pBlock);
        Destroy(pBlock.gameObject);
    }
    private void ConstructGrid()
    {
        var childCount = shipLayout.childCount;

        float targetMass = 0f;

        var multiSizeBlocks = new List<IMultiSizeBlockObject>();
        var blocks = new List<IBlockObject>();
        //list of all x and y positions of all blocks
        var xPositions = new List<float>();
        var yPositions = new List<float>();
        //iterates through all blocks and adds the positions to their respective list
        //total ship mass calculations
        for (int i = 0; i < childCount; i++)
        {
            var child = shipLayout.transform.GetChild(i);

            AddStatsToGrid(child);

            

            var multiSizeBlock = (IMultiSizeBlock)child.GetComponent(typeof(IMultiSizeBlock));
            if (multiSizeBlock != null)
            {
                var multiSizeBlockObject = new IMultiSizeBlockObject(multiSizeBlock, child);
                multiSizeBlocks.Add(multiSizeBlockObject);
                multiSizeBlock.blockBaseClass.shipGrid = this;
                var positions = multiSizeBlockObject.GetPositionsOfMultiSizeBlock();
                foreach (var position in positions)
                {
                    xPositions.Add(position.x);
                    yPositions.Add(position.y);
                }
                targetMass += multiSizeBlock.blockBaseClass.mass;
            }
            else
            {
                var block = (IBlock)child.GetComponent(typeof(IBlock));
                if (block != null)
                {
                    blocks.Add(new IBlockObject(block, child));
                    xPositions.Add(child.transform.localPosition.x);
                    yPositions.Add(child.transform.localPosition.y);
                    targetMass += block.blockBaseClass.mass;
                    block.blockBaseClass.block = block;
                    block.blockBaseClass.shipGrid = this;
                }
            }

            

        }
        _rb2d.mass = targetMass;
        //lowest abd highest offset of block positions from 0,0
        lowest = new Vector2Int((int)Mathf.Min(xPositions.ToArray()), (int)Mathf.Min(yPositions.ToArray()));
        highest = new Vector2Int((int)Mathf.Max(xPositions.ToArray()), (int)Mathf.Max(yPositions.ToArray()));
        //target size in width and height of grid
        Vector2Int gridSize = highest - lowest + new Vector2Int(1, 1);

        _shipGrid = new GameObject[gridSize.x, gridSize.y];

        //holds the local positions and mass of all blocks
        var posBlockData = new List<PosBlockData>();
        //ship grid population
        for (int i = 0; i < childCount; i++)
        {
            var child = shipLayout.transform.GetChild(i);
            var multiSizeBlock = (IMultiSizeBlock)child.GetComponent(typeof(IMultiSizeBlock));
            if (multiSizeBlock != null)
            {
                posBlockData.Add(new PosBlockData(child.localPosition, multiSizeBlock.blockBaseClass.mass));
                foreach (var vector2 in new IMultiSizeBlockObject(multiSizeBlock, child.transform).GetPositionsOfMultiSizeBlock())
                {
                    AddToGrid(child, vector2);
                }
            }
            else
            {
                var block = (IBlock)child.GetComponent(typeof(IBlock));
                if (block != null)
                {
                    posBlockData.Add(new PosBlockData(child.localPosition, block.blockBaseClass.mass));
                    AddToGrid(child);
                }
            }
        }
        centerOfMass = posBlockData.WeightedAverage();
        shipLayout.localPosition = -centerOfMass;
        #region debugging
        if (PlayerPrefs.Instance.debug3)
        {
            Debug.Log(lowest);
            Debug.Log(highest);
            Debug.Log(gridSize);
        }
        if (PlayerPrefs.Instance.debug4)
        {
            Debug.Log("Child count of ship grid is : " + childCount);
            Debug.Log("count of block objects in " + _ship.name + " is : " + blocks.Count);
            Debug.Log("count of multi size block objects in " + _ship.name + " is : " + multiSizeBlocks.Count);
            int count = 0;
            for (int x = 0; x < _shipGrid.GetLength(0); x++)
            {
                for (int y = 0; y < _shipGrid.GetLength(1); y++)
                {
                    if (_shipGrid[x, y] != null)
                    {
                        count++;
                    }
                }
            }
            Debug.Log("Count of occupied grid tiles in " + _ship.name + " is : " + count);
        }
        if (PlayerPrefs.Instance.debug7)
        {
            Debug.Log(centerOfMass);
        }
        if (PlayerPrefs.Instance.debug9)
        {
            Debug.Log(blockGrid.countInGrid);
        }
        #endregion
        
        
    }
    private bool IndexIsOutsideGridBounds(int[] index)
    {
        for (int x = 0; x < index.Length; x++)
        {
            if (index[x] > _shipGrid.GetLength(x)) return true;
            if (index[x] < 0) return true;
        }
        return false;
    }
}