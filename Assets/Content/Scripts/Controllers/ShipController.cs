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

    private GameObject _target;
    private Camera _camera;
    public bool[] thrusterGroupFiring = new bool[4];
    private ShipGrid shipGrid;
    #endregion
    private void Awake()
    {
        _camera = Camera.main;
        _camera.gameObject.GetComponent<CameraFollower>().SetTarget(gameObject);
        _ship = transform.parent.gameObject;
        shipGrid = _ship.GetComponent<ShipGrid>();
    }
    void Update()
    {
        switch (aimingMode)
        {
            case AimingMode.Cursor:
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    _ship.transform.rotation = RotationUtilities.MouseLookAtRotation(_ship, shipGrid.turningRate.leftTurningRate, shipGrid.turningRate.rightTurningRate);
                }
                break;
            case AimingMode.Keyboard:
                break;
        }
    }
    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        thrusterGroupFiring = new bool[4] { false, false, false, false };
        if (dampeningMode == DampeningMode.On)
        {
            //Dampening.Dampen(input, _rb2d, _ship.transform.rotation.eulerAngles.z, _ship, thrust);
        }
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
                shipGrid.FireThrusterGroup(i);
            }
            else
            {
                ShipControllerUitlities.SetThrusterGroupFlame(shipGrid._thrusterGroups[i], false);
            }
        }
        //dampen ship movement with thruster force pointing in the opposite direction of current velocity;
    }
}   
