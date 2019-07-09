#pragma warning disable 649
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GridUtilities.GridReader;

public class ShipGrid : MonoBehaviour
{
    public Color ThemeColor;

    private GameObject _ship;
    [SerializeField]
    private Rigidbody2D _rb2d;
    public Rigidbody2D rb2d
    {
        get => _rb2d;
        set => _rb2d = value;
    }

    public List<ITurret> turrets = new List<ITurret>();

    public List<IPowerConsumer> powerConsumers = new List<IPowerConsumer>();
    public List<IPowerGenerator> powerGenerators = new List<IPowerGenerator>();

    [HideInInspector]
    public float totalPowerGeneration;
    [HideInInspector]
    public float totalPowerConsumption;

    [SerializeField]
    private Transform _shipLayout;
    public Transform shipLayout => _shipLayout;
    [SerializeField]
    private Transform _grid;
    public Transform grid => _grid;

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
        public void Clear()
        {
            _turningRate = 0;
        }
    }

    public TurningRate turningRate;

    public int gridID;

    public NewThrust newThrust;

    [SerializeField]
    private MonoBehaviour _controller;
    public IController controller;

    public BlockGrid blockGrid;

    private void Awake()
    {
        blockGrid = new BlockGrid(this);
    }

    private void Start()
    {
        _ship = gameObject;
        gridID = transform.GetRootGridID();
        newThrust = new NewThrust(this);
        turningRate = new TurningRate();
        new GridManager.shipReference(gameObject, this, _rb2d).AddToTable(gridID);
        ConstructGrid();
        controller = (IController)_controller;
    }

    public void LoadBlueprint(GridUtilities.Blueprint blueprint)
    {
        if (blueprint.valid)
        {
            blockGrid.blockList.ForEach(block => RemoveFromGrid(block.transform));
            transform.name = blueprint.name;
            foreach (var block in blueprint.blocks)
            {
                var pos = block.transform.localPosition;
                var rot = block.transform.localRotation;
                block.transform.parent = shipLayout;
                block.transform.localPosition = pos;
                block.transform.localRotation = rot;
                block.SetActive(true);
            }
            ConstructGrid();
        }
        else
        {
            Debug.LogError("Invalid blueprint given");
        }
    }

    public void AddStatsToGrid(Transform pBlock, bool updateCenterOfMass = false)
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
        block.blockBaseClass.shipGrid = this;
        block.blockBaseClass.parentClass = (MonoBehaviour)component;
        block.blockBaseClass.block = block;

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
                thruster.trailManager.particleColor = ThemeColor;
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

        if (updateCenterOfMass)
            blockGrid.UpdateCenterOfMass();
    }

    public void RemoveStatsFromGrid(Transform pBlock, bool updateCenterOfMass = false)
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

        blockGrid.RemoveFromGrid(block);

        if (updateCenterOfMass)
            blockGrid.UpdateCenterOfMass();
    }

    public void RemoveFromGrid(Transform pBlock)
    {
        RemoveStatsFromGrid(pBlock);
        Destroy(pBlock.gameObject);
    }

    public void ClearGrid()
    {
        var childCount = shipLayout.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var child = shipLayout.transform.GetChild(i);
            RemoveFromGrid(child);
        }
    }

    private void ConstructGrid()
    {
        var childCount = shipLayout.childCount;
        var children = new List<GameObject>();

        for (int i = 0; i < childCount; i++)
            children.Add(shipLayout.transform.GetChild(i).gameObject);
        
        Initialize(children);
    }

    private void Initialize(List<GameObject> blocks)
    {
        var Count = blocks.Count;

        blocks.ForEach(block => AddStatsToGrid(block.transform));

        if (PlayerPrefs.Instance.debug9)
            Debug.Log(blockGrid.countInGrid);

        blockGrid.UpdateCenterOfMass();
    }
}