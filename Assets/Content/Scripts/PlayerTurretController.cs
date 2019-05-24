using UnityEngine;
public class PlayerTurretController : MonoBehaviour
{
    private Turret turret;
    private void Awake()
    {
        turret = GetComponent<Turret>();
    }
    private void Update()
    {
        turret.turret.transform.rotation = RotationUtilities.MouseLookAtRotation(turret.turret, turret.turretSpeed);
        if (Input.GetMouseButton(0) && turret.GetCanFire()) turret.Fire();
    }
}
