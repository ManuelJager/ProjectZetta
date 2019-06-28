#pragma warning disable 649
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
    [SerializeField]
    private GameObject _ship;
    [SerializeField]
    private Transform _grid;
    [SerializeField]
    private Transform _cameraAnchorPoint;
    private GameObject _target;
    private Camera _camera;
    public bool[] thrusterGroupFiring = new bool[4];
    [SerializeField]
    private ShipGrid _shipGrid;
    [SerializeField]
    private UIBar _powerConsumptionBar;
    private float _currentConsumption;
    public float currentConsumption
    {
        get => _currentConsumption;
        set => _currentConsumption = value;
    }
    public static ShipController Instance;

    #endregion
    private void Start()
    {
        _cameraAnchorPoint.SetThisAsCameraTarget();
        if (Instance == null)
            Instance = this;
    }
    public void Update()
    {
        var rotationFromShipToMouse = RotationUtilities.GetMouseWorldPos(_grid);
        var shipZRotation = 360 - _grid.rotation.eulerAngles.z;
        //aiming
        switch (aimingMode)
        {
            case AimingMode.Cursor:
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    var rot = RotationUtilities.MouseLookAtRotation(_grid, _shipGrid.turningRate.turningRate, _shipGrid.turningRate.turningRate, rotationFromShipToMouse);
                    _grid.transform.rotation = rot;
                }
                break;
            case AimingMode.Keyboard:
                break;
        }
        foreach (var turret in _shipGrid.turrets)
        {
            turret.turretObject.transform.rotation = RotationUtilities.MouseLookAtRotation(turret.turretObject.transform, turret.turretSpeed);
            if (Input.GetMouseButton(0) && turret.hasReloaded) turret.Fire();
        }
        //movement
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float inputAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        if (PlayerPrefs.Instance.clearLog)
            Common.ClearLog();
        //normalizes inputAngle
        if (inputAngle < 0f)
            inputAngle += 360f;
        var thrustRotation = inputAngle;
        if (PlayerPrefs.Instance.debug8)
            Debug.Log("input angle is : " + inputAngle);
        var mouseRot = 360 - rotationFromShipToMouse.eulerAngles.z;
        thrustRotation.AddAngleRef(mouseRot.AddAngle(-shipZRotation));
        if (PlayerPrefs.Instance.debug8)
            Debug.Log("thrustRotation is : " + thrustRotation);
        if (input != Vector2.zero)
            _shipGrid.newThrust.FireThrustersInDirection(thrustRotation);
        _powerConsumptionBar.val = currentConsumption;
        _powerConsumptionBar.UIUpdate();
        currentConsumption = 0;
    }
}   
