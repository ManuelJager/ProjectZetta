using System.Collections.Generic;
using UnityEngine;
public static class ShipControllerUitlities
{
    public static void ApplyRB2DForce(Rigidbody2D rb2d, Transform transform, NewThrust newThrust, float multiplier, Common.Orientation orientation)
    {
        switch (orientation)
        {
            case Common.Orientation.forward:
                rb2d.AddForce(transform.right * newThrust.thrustVectors[orientation].thrust * Time.deltaTime);
                break;
            case Common.Orientation.backward:
                rb2d.AddForce(transform.right * newThrust.thrustVectors[orientation].thrust * -1f * Time.deltaTime);
                break;
            case Common.Orientation.left:
                rb2d.AddForce(transform.up * newThrust.thrustVectors[orientation].thrust * Time.deltaTime);
                break;
            case Common.Orientation.right:
                rb2d.AddForce(transform.up * newThrust.thrustVectors[orientation].thrust * -1f * Time.deltaTime);
                break;
        }
    }
}
