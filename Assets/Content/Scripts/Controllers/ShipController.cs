using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public struct Thrust
    {
        public float forwardThrust;
        public float backwardsThrust;
        public float leftThrust;
        public float rightThrust;

        public Thrust(float forwardThrust = 3f, float backwardsThrust = 1f, float leftThrust = 1f, float rightThrust = 1f)
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

        public TurningRate(float leftTurningRate = 1f, float rightTurningRate = 1f)
        {
            this.leftTurningRate = leftTurningRate;
            this.rightTurningRate = rightTurningRate;
        }
    }
    public TurningRate turningRate;

    private GameObject _ship;
    private Rigidbody2D _rb2d;
    private GameObject _target;
    private Camera _camera;

    private float movementFactor = 3;
    private float movementThreshold = 10;
    private float accel = 10;
    #endregion
    private void Start()
    {
        SetThrustVectors();
        SetTurningRateVectors();
        _ship = transform.parent.gameObject;
        _rb2d = _ship.GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _target = GameObject.Find("Target");
        _camera.gameObject.GetComponent<CameraFollower>().SetTarget(_ship);
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
        //rotates the ship towards the cursor
        


        //_ship.transform.rotation = RotationUtilities.ObjectLookAtRotation(_ship, _target, 10f);
    }

    void FixedUpdate()
    {
        Vector2 input = Input.

        _ship.transform.Translate(movementFactor * thrust.forwardThrust, 0.0f, 0.0f);
    }
    public void SetThrustVectors(float[] thrustVectors = null)
    {
        //if no argument has been given, thrust vectors will be set to default values
        thrustVectors = thrustVectors ?? new float[4] { 0.1f, 1f, 1f, 1f };
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
}
