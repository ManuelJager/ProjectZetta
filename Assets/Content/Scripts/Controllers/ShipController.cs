using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipController : MonoBehaviour, IController
{
    #region declarations
    public enum AimingMode
    {
        Cursor,
        Keyboard
    }
    private AimingMode _aimingMode = AimingMode.Cursor;
    public AimingMode aimingMode
    {
        get
        {
            return _aimingMode;
        }
        set
        {
            _aimingMode = value;
        }
    }
    public enum DampeningMode
    {
        On,
        Off
    }
    private DampeningMode _dampeningMode = DampeningMode.On;
    public DampeningMode dampeningMode
    {
        get
        {
            return _dampeningMode;
        }
        set
        {
            _dampeningMode = value;
        }
    }
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
    [SerializeField]
    private GameObject _ship;
    [SerializeField]
    private Rigidbody2D _rb2d;
    private GameObject _target;
    private Camera _camera;
    //0 forward , 1 backward, 2 left, 3 right
    private List<GameObject>[] _thrusterGroups;

    #endregion
    private void Start()
    {
        SetTurningRateVectors();
        _camera = Camera.main;
        _camera.gameObject.GetComponent<CameraFollower>().SetTarget(_ship);
        //initialize thrusterGroups
        _thrusterGroups = new List<GameObject>[4];
        for (int i = 0; i < 4; i++)
        {
            _thrusterGroups[i] = new List<GameObject>();
        }
        SetThrusterGroups();
        SetThrustVectors(ShipControllerUitlities.CalculateThrustVectors(_thrusterGroups));
    }
    void Update()
    {
        switch (aimingMode)
        {
            case AimingMode.Cursor:
                _ship.transform.rotation = RotationUtilities.MouseLookAtRotation(_ship, _camera, turningRate.leftTurningRate, turningRate.rightTurningRate);
                break;
            case AimingMode.Keyboard:
                break;
        }
    }
    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool[] thrusterGroupFiring = { false, false, false, false };
        if (input.x > 0)
        {
            thrusterGroupFiring[2] = true;
        }
        if (input.x < 0)
        {
            thrusterGroupFiring[3] = true;
        }
        if (input.y > 0)
        {
            thrusterGroupFiring[0] = true;
        }
        if (input.y < 0)
        {
            thrusterGroupFiring[1] = true;
        }
        for (int i = 0; i < 4; i++)
        {
            if (thrusterGroupFiring[i])
            {
                FireThrusterGroup(i);
            }
            else
            {
                ShipControllerUitlities.SetThrusterGroupFlame(_thrusterGroups[i], false);
            }
        }
        //dampen ship movement with thruster force pointing in the opposite direction of current velocity;
        if (dampeningMode == DampeningMode.On)
        {
            //Dampening.Dampen(input, _rb2d, _ship.transform.rotation.eulerAngles.z, _ship, thrust);
        }
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
    private void FireThrusterGroup(int group)
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
