using UnityEngine;
public class PlayerTurretController : MonoBehaviour
{
    private ITurret turret;
    private void Awake()
    {
        turret = (ITurret)GetComponent(typeof(ITurret));
    }
    private void Update()
    {
        turret.turretObject.rotation = RotationUtilities.MouseLookAtRotation(turret.turretObject.transform, turret.turretSpeed);
        if (Input.GetMouseButton(0) && turret.hasReloaded) turret.Fire();
    }
}
