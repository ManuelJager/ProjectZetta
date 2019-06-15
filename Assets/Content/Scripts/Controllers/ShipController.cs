#pragma warning disable 649
using UnityEngine;
public class ShipController : MonoBehaviour
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
    private ShipGrid _shipGrid;
    
    #endregion
    private void Awake()
    {
        _ship = transform.parent.gameObject;
        _shipGrid = _ship.GetComponent<ShipGrid>();
    }
    private void Start()
    {
        _cameraAnchorPoint.SetThisAsCameraTarget();
        //UIManager.Instance.energyBar.maxVal = _shipGrid.totalPowerConsumption;
    }
    void Update()
    {
        var q = RotationUtilities.GetMouseWorldPos(_grid);
        //aiming
        switch (aimingMode)
        {
            case AimingMode.Cursor:
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    var rot = RotationUtilities.MouseLookAtRotation(_grid, _shipGrid.turningRate.leftTurningRate, _shipGrid.turningRate.rightTurningRate, q);
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
        if (inputAngle < 0f)
            inputAngle += 360f;
        Debug.Log("Input angle is : " + inputAngle);
        if (input != Vector2.zero)
            _shipGrid.newThrust.FireThrusterInDirection(_shipGrid.transform.rotation.eulerAngles.z, inputAngle);
    }
}   
