using System.Collections.Generic;
using UnityEngine;
public class ShipGrid : MonoBehaviour
{
    private GameObject _ship;
    private Rigidbody2D _rb2d;

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
    private void Awake()
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
        SetUpgrid();
    }

    private void SetUpgrid()
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
}
