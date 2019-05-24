using System;
using System.Collections.Generic;
using UnityEngine;
public class ShipGrid : MonoBehaviour
{
    private GameObject _ship;
    private Rigidbody2D _rb2d;
    private GameObject[,] _shipGrid;
    private Vector2Int lowest;
    private Vector2Int highest;

    public struct Thrust
    {
        public float forwardThrust;
        public float backwardsThrust;
        public float leftThrust;
        public float rightThrust;

        public Thrust(float forwardThrust, float backwardsThrust, float leftThrust, float rightThrust)
        {
            this.forwardThrust = forwardThrust;
            this.backwardsThrust = backwardsThrust;
            this.leftThrust = leftThrust;
            this.rightThrust = rightThrust;
        }
    }
    public Thrust thrust;
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
    public TurningRate turningRate;
    public List<GameObject>[] _thrusterGroups;
    private void Start()
    {
        _ship = gameObject;
        _rb2d = GetComponent<Rigidbody2D>();
        _thrusterGroups = new List<GameObject>[4];
        for (int i = 0; i < 4; i++)
        {
            _thrusterGroups[i] = new List<GameObject>();
        }
        SetThrusterGroups();
        SetTurningRateVectors();
        SetThrustVectors(ShipControllerUitlities.CalculateThrustVectors(_thrusterGroups));
        CalculateMass();
        ConstructGrid();
    }
    private void CalculateMass()
    {
        List<IBlock> blocks = new List<IBlock>();
        var shipLayout = _ship.transform.GetChild(0).GetChild(0);
        var count = shipLayout.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = shipLayout.transform.GetChild(i);
            //interface cast
            blocks.Add((IBlock)child.GetComponent(typeof(IBlock)));
        }
        float targetMass = 0f;
        foreach (var block in blocks)
        {
            targetMass += block.GetMass();
        }
        _rb2d.mass = targetMass;
    }

    public void SetThrustVectors(float[] thrustVectors = null)
    {
        //if no argument has been given, thrust vectors will be set to default values
        thrustVectors = thrustVectors ?? new float[4] { 150f, 100f, 100f, 100f };
        thrust.forwardThrust = thrustVectors[0];
        thrust.backwardsThrust = thrustVectors[1];
        thrust.leftThrust = thrustVectors[2];
        thrust.rightThrust = thrustVectors[3];
    }
    public float[] GetThrustVectors()
    {
        var thrustVectors = new float[4];
        thrustVectors[0] = thrust.forwardThrust;
        thrustVectors[1] = thrust.backwardsThrust;
        thrustVectors[2] = thrust.leftThrust;
        thrustVectors[3] = thrust.rightThrust;
        return thrustVectors;
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
    public void SetThrusterGroups()
    {
        var shipLayout = _ship.transform.GetChild(0).GetChild(0);
        var count = shipLayout.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = shipLayout.transform.GetChild(i);
            if (child.tag == "Thruster")
            {
                AddToThrusterGroups(child.gameObject);
            }
        }
    }
    private void AddToThrusterGroups(GameObject thruster)
    {
        switch (thruster.transform.rotation.eulerAngles.z)
        {
            case 0f:
                _thrusterGroups[0].Add(thruster);
                break;
            case 90f:
                _thrusterGroups[3].Add(thruster);
                break;
            case 180f:
                _thrusterGroups[1].Add(thruster);
                break;
            case 270f:
                _thrusterGroups[2].Add(thruster);
                break;
        }
    }
    public void FireThrusterGroup(int group)
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
        ShipControllerUitlities.ApplyRB2DForce(_rb2d, _ship, thrust, orientation);
        ShipControllerUitlities.SetThrusterGroupFlame(_thrusterGroups[group], true);
    }
    public void AddToGrid(Transform block)
    {
        var pos = block.position;
        var targetXIndex = (int)pos.x - lowest.x;
        var targetYIndex = (int)pos.y - lowest.y;
        if (!IndexIsOutsideGridBounds(new int[] { targetXIndex, targetYIndex }))
        {
            _shipGrid[targetXIndex, targetYIndex] = block.gameObject;
        }
        else
        {
            Debug.LogWarning("Block is outside of bounds");
        }
    }
    private void ConstructGrid()
    {
        var shipLayout = _ship.transform.GetChild(0).GetChild(0);
        var count = shipLayout.childCount;
        //list of all x and y positions of all blocks
        List<float> xPositions = new List<float>();
        List<float> yPositions = new List<float>();
        //iterates through all blocks and adds the positions to their respective list
        for (int i = 0; i < count; i++)
        {
            var child = shipLayout.transform.GetChild(i);
            xPositions.Add(child.transform.localPosition.x);
            yPositions.Add(child.transform.localPosition.y);
        }
        //lowest abd highest offset of block positions from 0,0
        lowest = new Vector2Int((int)Mathf.Min(xPositions.ToArray()), (int)Mathf.Min(yPositions.ToArray()));
        highest = new Vector2Int((int)Mathf.Max(xPositions.ToArray()), (int)Mathf.Max(yPositions.ToArray()));
        //target size in width and height of grid
        Vector2Int gridSize = highest - lowest + new Vector2Int(1, 1);

        _shipGrid = new GameObject[gridSize.x, gridSize.y];

        for (int i = 0; i < count; i++)
        {
            var child = shipLayout.transform.GetChild(i);
            AddToGrid(child);
        }
        //debug
        if (PlayerPrefs.Instance.debug3)
        {
            Debug.Log(lowest);
            Debug.Log(highest);
            Debug.Log(gridSize);
        }
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