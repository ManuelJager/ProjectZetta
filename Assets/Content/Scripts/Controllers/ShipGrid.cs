using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ShipGrid : MonoBehaviour
{ 
    enum type
    {
        single,
        multiple
    }
    public struct PosBlockData
    {
        public Vector2 gridPosition;
        public float mass;

        public PosBlockData (Vector2 gridPosition, float mass)
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
    private Rigidbody2D _rb2d;
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
    public struct TurningRate
    {
        public float leftTurningRate;
        public float rightTurningRate;
        public TurningRate(float leftTurningRate, float rightTurningRate)
        {
            this.leftTurningRate = leftTurningRate;
            this.rightTurningRate = rightTurningRate;
        }
    }
    private Transform _shipLayout;
    public Transform shipLayout => _shipLayout != null ? _shipLayout : _shipLayout = transform.GetChild(0).GetChild(0);
    
    public TurningRate turningRate;
    public List<IThruster>[] _thrusterGroups;
    public Vector2 centerOfMass;
    public int gridID;

    public List<IThruster> thrusters = new List<IThruster>();
    public NewThrust newThrust;
    private void Start()
    {
        _ship = gameObject;
        _rb2d = GetComponent<Rigidbody2D>();
        gridID = transform.GetRootGridID();
        new GridManager.shipReference(gameObject, this, _rb2d).AddToTable(gridID);
        _thrusterGroups = new List<IThruster>[4];
        for (int i = 0; i < 4; i++)
        {
            _thrusterGroups[i] = new List<IThruster>();
        }
        ConstructGrid();
        SetTurningRateVectors();
        newThrust = new NewThrust(thrusters);
    }
    [Obsolete("mass calculations are automatically done in ContructGrid now")]
    private void CalculateMass()
    {
        List<IMultiSizeBlockObject> multiSizeBlocks = new List<IMultiSizeBlockObject>();
        List<IBlockObject> blocks = new List<IBlockObject>();
        var shipLayout = _ship.transform.GetChild(0);
        var count = shipLayout.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = shipLayout.transform.GetChild(i);
            //interface cast
            var block = (IBlock)child.GetComponent(typeof(IBlock));
            if (block != null)
            {
                blocks.Add(new IBlockObject());
            }
        }
        if (PlayerPrefs.Instance.debug4)
        {
            Debug.Log("count of blocks in " + _ship.name + " is : " + blocks.Count);
            Debug.Log("count of multi size blocks in " + _ship.name + " is : " + multiSizeBlocks.Count);
        }
        float targetMass = 0f;
        foreach (var block in blocks)
        {
            targetMass += block.block.blockBaseClass.mass;
        }
        _rb2d.mass = targetMass;
    }
    public void RemoveFromThrustGroup(IThruster thrusterToBeRemoved)
    {
        for (int i = 0; i < 4; i++)
        {
            _thrusterGroups[i].Remove(thrusterToBeRemoved);
        }
    }
    public void SetTurningRateVectors(float[] turningRateVectors = null)
    {
        //if no argument has been given, turning rate vectors will be set to default values
        turningRateVectors = turningRateVectors ?? new float[2] { 100f, 100f };
        turningRate.leftTurningRate = turningRateVectors[0];
        turningRate.rightTurningRate = turningRateVectors[1];
    }
    public float[] GetTurningRateVectors()
    {
        var turningRateVectors = new float[2];
        turningRateVectors[0] = turningRate.leftTurningRate;
        turningRateVectors[1] = turningRate.rightTurningRate;
        return turningRateVectors;
    }
    public void FireThrusterGroup(int group, float multiplier)
    {
        var orientation = new Common.Orientation();
        switch (group)
        {
            case 0:
                orientation = Common.Orientation.forward;
                break;
            case 1:
                orientation = Common.Orientation.backward;
                break;
            case 2:
                orientation = Common.Orientation.right;
                break;
            case 3:
                orientation = Common.Orientation.left;
                break;
        }
        ShipControllerUitlities.ApplyRB2DForce(_rb2d, transform, newThrust, multiplier, orientation);
        ShipControllerUitlities.SetThrusterGroupFlame(_thrusterGroups[group], true);
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
                Debug.LogWarning("ShipGrid at : " + new Vector2(targetXIndex, targetYIndex) + " is already assigned by " + _shipGrid[targetXIndex, targetYIndex].name + ", name of block is : " + block.name );
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
            var turret = (ITurret)child.GetComponent(typeof(ITurret));
            if (turret != null)
                turrets.Add(turret);
            var thruster = (IThruster)child.GetComponent(typeof(IThruster));
            if (thruster != null)
            {
                thrusters.Add(thruster);
                //AddToThrusterGroups(thruster);
            }
            var multiSizeBlock = (IMultiSizeBlock)child.GetComponent(typeof(IMultiSizeBlock));
            if (multiSizeBlock != null)
            {
                var multiSizeBlockObject = new IMultiSizeBlockObject(multiSizeBlock, child);
                multiSizeBlocks.Add(multiSizeBlockObject);
                multiSizeBlock.multiSizeBlockBaseClass.parentClass = (MonoBehaviour)multiSizeBlock;
                multiSizeBlock.blockBaseClass.gridID = gridID;
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
                    block.blockBaseClass.parentClass = (MonoBehaviour)block;
                    block.blockBaseClass.gridID = gridID;
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
                    if (_shipGrid[x,y] != null)
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
        #endregion
    }
    public void RemoveFromPowerConsumption(IPowerConsumer item)  => totalPowerConsumption -= item.powerConsumption;
    public void AddToPowerConsumption     (IPowerConsumer item)  => totalPowerConsumption += item.powerConsumption;
    public void RemoveFromPowerGeneration (IPowerGenerator item) => totalPowerGeneration  -= item.powerGeneration;
    public void AddToPowerGeneration      (IPowerGenerator item) => totalPowerGeneration  += item.powerGeneration;
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